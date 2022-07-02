using System;
using Android.Content;

using System.Net;
using System.IO;

namespace Tomoor.Droid.Utility
{
    public static class ShareIt
    {
        //https://stackoverflow.com/questions/14450867/branching-the-android-share-intent-extras-depending-on-which-method-they-choose
        //https://stackoverflow.com/questions/38142699/android-intent-send-with-text-and-link-to-messenger-wont-work

            // share image with link
        public static void Share(Context ctx, string title, string content, string imageUrl)
        {
            try
            {
                // Boolean isSDPresent = android.os.Environment.getExternalStorageState().equals(android.os.Environment.MEDIA_MOUNTED);
                bool isSDPresent = Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted);
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) )
                    return;

                if (!isSDPresent)
                {
                    var sharingIntent = new Intent();
                    sharingIntent.SetAction(Intent.ActionSend);
                    sharingIntent.SetType("*/*");

                    sharingIntent.PutExtra(Intent.ExtraText, content);
                  //  sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
                    //sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    ctx.StartActivity(Intent.CreateChooser(sharingIntent, title));
                    return;
                }
                #region DownloadUrl
                Uri _url = new Uri(imageUrl);
                string[] urlParts = imageUrl.Split('/');
                if (urlParts.Length != 7)
                {
                    return;
                }
                string _FileName = urlParts[6];
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                var filePath = Path.Combine(sdCardPath, _FileName);
                if (File.Exists(filePath))
                {
                    var imageUri = Android.Net.Uri.Parse($"file://{sdCardPath}/{_FileName}");
                    //Uri uri = Uri.fr
                    var sharingIntent = new Intent();
                    sharingIntent.SetAction(Intent.ActionSend);
                    sharingIntent.SetType("image/* | application/twitter | */*");

                    sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
                    sharingIntent.PutExtra(Intent.ExtraText, content);
                    //sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    ctx.StartActivity(Intent.CreateChooser(sharingIntent, title));
                }
                else
                {
                    WebClient _client = new WebClient();
                    _client.DownloadDataAsync(_url);
                    _client.DownloadDataCompleted += (s, e) => {
                        var Ex = e.Error;
                        if (Ex == null)
                        {
                            var bytes = e.Result;
                            File.WriteAllBytes(filePath, bytes);
                            //  Android.Net.Uri imageUri = Android.Net.Uri.FromFile(new Java.IO.File(localPath));
                            // Android.Net.Uri imageUri = Android.Net.Uri.Parse($"file:/{localPath}");

                            // var uri = Android.Net.Uri.FromFile(new Java.IO.File(localPath));
                            var imageUri = Android.Net.Uri.Parse($"file://{sdCardPath}/{_FileName}");
                            //Uri uri = Uri.fr
                            var sharingIntent = new Intent();
                            sharingIntent.SetAction(Intent.ActionSend);
                            sharingIntent.SetType("image/*");
                            
                            sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
                            sharingIntent.PutExtra(Intent.ExtraText, content);
                            //sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                            ctx.StartActivity(Intent.CreateChooser(sharingIntent, title));
                        }
                    };
                }

                #endregion
            }
            catch (Exception)
            {

                //throw;//x
            }
           


            //string ImageUrl;
           // var name = Application.Context.Resources.GetResourceName(Resource.Drawable.icon_120).Replace(':', '/');
           // var imageUri = Uri.Parse("android.resource://" + name);
            
        }

        // share just link
        public static void Share(Context ctx, string title, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                    return;

                var sharingIntent = new Intent();
                sharingIntent.SetAction(Intent.ActionSend);
                sharingIntent.SetType("*/*");
                sharingIntent.PutExtra(Intent.ExtraText, content);
                ctx.StartActivity(Intent.CreateChooser(sharingIntent, title));
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}