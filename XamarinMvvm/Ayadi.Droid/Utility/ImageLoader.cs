using System;
using System.Net;
using System.IO;
using Android.Widget;
using Android.Graphics;
using Android.Content;

namespace Ayadi.Droid.Utility
{
    public static class ImageLoader
    {
        public static void LoadImage(Context context, string Url, ImageView ImgView, int sampleSize)
        {
            try
            {
                if (Url == null)
                {
                    ImgView.SetImageResource(Resource.Drawable.DefultImg);
                    return;
                }
                int index_ = Url.LastIndexOf('/');
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //string[] urlParts = Url.Split('/');
                //if (urlParts.Length != 7)
                //{
                //    ImgView.SetImageResource(Resource.Drawable.bunner4);
                //    return;
                //}
                //string _FileName = urlParts[6];
                string _FileName = Url.Substring(index_ +1);
                string localPath = System.IO.Path.Combine(documentsPath, _FileName);
                if (File.Exists(localPath))
                {
                    Bitmap SavedBitmap = GetBitMap(localPath, sampleSize);
                    if (SavedBitmap != null)
                    {
                        ImgView.SetImageBitmap(SavedBitmap);
                        //SavedBitmap.Recycle();
                    }
                }
                else
                {
                    Uri _url = new Uri(Url);
                    WebClient _client = new WebClient();
                    _client.DownloadDataAsync(_url);
                    _client.DownloadDataCompleted += (s, e) => {
                        var Ex = e.Error;
                        if (Ex == null)
                        {
                            var bytes = e.Result;
                            File.WriteAllBytes(localPath, bytes);
                            Bitmap SavedBitmap = GetBitMap(localPath, sampleSize);
                            if (SavedBitmap != null)
                            {
                                ImgView.SetImageBitmap(SavedBitmap);
                                //SavedBitmap.Recycle();
                            }

                        }
                    };
                    // ImgView.SetImageResource(Resource.Drawable.Talabatk_Logo);
                }
            }
            catch
            {
                //ImgView.Image = UIImage.FromFile("Images/logo.png");
                ImgView.SetImageResource(Resource.Drawable.DefultImg);
            }
        }

        private static Bitmap GetBitMap(string localPath, int SampleSize)
        {
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InSampleSize = SampleSize;
                options.InPreferredConfig = Bitmap.Config.Argb8888;

                return BitmapFactory.DecodeFile(localPath, options);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Iamger GetBitmap", ex.Message);
                return null;
            }

        }
    }
}