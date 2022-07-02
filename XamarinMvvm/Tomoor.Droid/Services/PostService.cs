

namespace App.Activities.AddPost.Service
{ 
    [Service(Exported = false)]
    public class PostService : Android.App.Service
    {
        #region Variables Basic

        public string ActionPostService;
        public static string ActionPost;
        private static string PagePost;
        private static PostService Service;
         
        private TabbedMainActivity GlobalContextTabbed;
        public FileModel DataPost;


        #endregion

        #region General

        public static PostService GetPostService()
        {
            return Service;
        }
         
        public override IBinder OnBind(Intent intent)
        {
            return null!;
        }
         
        public override void OnCreate()
        {
            try
            {
                base.OnCreate();
                Service = this;

                
                GlobalContextTabbed = TabbedMainActivity.GetInstance();
                MNotificationManager = (NotificationManager)GetSystemService(NotificationService);

                Create_Progress_Notification();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            try
            {
                base.OnStartCommand(intent, flags, startId);

                ActionPostService = intent.Action;
                var data = intent.GetStringExtra("DataPost");
                PagePost = intent.GetStringExtra("PagePost") ?? "";

               DataPost = JsonConvert.DeserializeObject<FileModel>(data);
                if (ActionPostService == ActionPost)
                {
                    if (DataPost != null)
                    {
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { AddPost });
                    }
                }
                return StartCommandResult.Sticky;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return StartCommandResult.NotSticky;
            }
        }

        public async Task AddPost()
        {
            try
            {
                var (apiStatus, respond) = await RequestsAsync.Posts.AddNewPostAsync(UserDetails.UserId , DataPost.IdPost, DataPost.PagePost, DataPost.Content, DataPost.PostPrivacy, DataPost.PostFeelingType, DataPost.PostFeelingText, DataPost.PlaceText, DataPost.AttachmentList, DataPost.AnswersList, DataPost.IdColor, DataPost.AlbumName);
                if (apiStatus == Status.Ok)
                {
                    if (respond is AddPostObject postObject)
                    {
                        ToastUtils.ShowToast(Application.Context, Application.Context.GetText(Resource.String.Lbl_Post_Added), ToastLength.Short);

                        GlobalContextTabbed?.RunOnUiThread(() =>
                        {
                            try
                            {
                                if (UserDetails.SoundControl)
                                    Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("PopNotificationPost.mp3");

                                if (postObject.PostData != null)
                                {
                                    postObject.PostData.Reaction = new Reaction();

                                    if (PagePost is PostType.Normal)
                                    {
                                        //var countList = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.ItemCount ?? 0;

                                        var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.ListDiffer, this);

                                        var check = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.ListDiffer?.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox /*&& a.TypeView != PostModelType.SearchForPosts*/);
                                        if (check != null)
                                            combine.CombineDefaultPostSections("Top");
                                        else
                                            combine.CombineDefaultPostSections();

                                        var emptyStateChecker = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.ListDiffer?.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                        if (emptyStateChecker != null && GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.ListDiffer?.Count > 1)
                                            GlobalContextTabbed.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                        //GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.NotifyItemRangeInserted(countIndex, GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count - countList);
                                        GlobalContextTabbed.NewsFeedTab.PostFeedAdapter?.NotifyDataSetChanged();

                                       
                                    }
                                    
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        });
                    }
                }
                else if (apiStatus == Status.VideoUploadWithProcessed)
                {
                    ToastUtils.ShowToast(this, GetText(Resource.String.Lbl_VideoUploadWithProcessed), ToastLength.Short);
                }
                else
                {
                    RemoveNotification();
                    Methods.DisplayReportResult(GlobalContextTabbed, respond);
                }

                RemoveNotification();
            }
            catch (Exception e)
            {
                RemoveNotification();
                Methods.DisplayReportResultTrack(e);
            }
        }

         
        #region Notification

        private readonly string NotificationChannelId = "wowonder_ch_1";
        private const int NotificationId = 2020;
        private NotificationManager MNotificationManager;
        private NotificationCompat.Builder NotificationBuilder;
        private RemoteViews NotificationView;
        private void Create_Progress_Notification()
        {
            try
            {
                MNotificationManager = (NotificationManager)GetSystemService(NotificationService);

                NotificationView = new RemoteViews(PackageName, Resource.Layout.ViewProgressNotification);

                Intent resultIntent = new Intent();

                PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, resultIntent, Build.VERSION.SdkInt >= BuildVersionCodes.M ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable : PendingIntentFlags.UpdateCurrent);

                NotificationBuilder = new NotificationCompat.Builder(this, NotificationChannelId);
                NotificationBuilder.SetSmallIcon(Resource.Mipmap.icon);
                NotificationBuilder.SetColor(ContextCompat.GetColor(this, Resource.Color.accent));
                NotificationBuilder.SetCustomContentView(NotificationView)
                    .SetOngoing(true)
                    .SetContentIntent(resultPendingIntent)
                    .SetDefaults(NotificationCompat.DefaultAll)
                    .SetPriority((int)NotificationPriority.High);

                NotificationBuilder.SetVibrate(new[] { 0L });
                NotificationBuilder.SetVisibility(NotificationCompat.VisibilityPublic);

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.O:
                    {
                        var importance = NotificationImportance.High;
                        NotificationChannel notificationChannel = new NotificationChannel(NotificationChannelId, AppSettings.ApplicationName, importance);
                        notificationChannel.EnableLights(false);
                        notificationChannel.EnableVibration(false);
                        NotificationBuilder.SetChannelId(NotificationChannelId);

                        MNotificationManager?.CreateNotificationChannel(notificationChannel);
                        break;
                    }
                }

                MNotificationManager?.Notify(NotificationId, NotificationBuilder.Build());
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RemoveNotification()
        {
            try
            {
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager?.Cancel(NotificationId);

                MNotificationManager?.CancelAll(); 
                StopSelf();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void UpdateNotification(string type)
        {
            try
            {
                switch (type)
                {
                    case "Post":
                        NotificationView.SetTextViewText(Resource.Id.title, GetString(Resource.String.Lbl_UploadingPost));
                        break;
                }

                MNotificationManager?.Notify(NotificationId, NotificationBuilder.Build());
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
    }  
}