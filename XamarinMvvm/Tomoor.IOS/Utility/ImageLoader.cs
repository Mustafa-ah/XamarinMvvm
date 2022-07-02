using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tomoor.IOS.Utility
{
    public static class ImageLoader
    {

        public static void LoadImage(string Url, UIImageView ImgView)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string[] urlParts = Url.Split('/');
                //if (urlParts.Length != 6)
                //{
                //    ImgView.Image = UIImage.FromFile("Images/DefultImg.png");
                //    return;
                //}
                string _FileName = urlParts[urlParts.Length - 1];
                string localPath = Path.Combine(documentsPath, _FileName);
                if (File.Exists(localPath))
                {
                    ImgView.Image = UIImage.FromFile(localPath);
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
                            ImgView.Image = UIImage.FromFile(localPath);
                        }

                    };
                }
            }
            catch
            {
                ImgView.Image = UIImage.FromFile("Images/DefultImg.png");
            }
        }

        public static void LoadTableImage(string Url, UIImageView ImgView)
        {
            try
            {
                //Console.WriteLine("......... urls.: " + Url);
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string[] urlParts = Url.Split('/');
                //if (urlParts.Length != 6)
                //{
                //    ImgView.Image = UIImage.FromFile("Images/logo.png");
                //    return;
                //}
                string _FileName = urlParts[urlParts.Length - 1];
                string localPath = Path.Combine(documentsPath, _FileName);
                if (File.Exists(localPath))
                {
                    ImgView.Image = UIImage.FromFile(localPath);
                }
                else
                {
                    Uri _url = new Uri(Url);
                    WebClient _client = new WebClient();
                    // _client.CancelAsync();
                    _client.DownloadDataAsync(_url);

                    _client.DownloadDataCompleted += (s, e) => {
                        var Ex = e.Error;
                        //if (e.Cancelled)
                        //{
                        //    // _client.
                        //    Console.WriteLine(".............dowanloading canceld ............");
                        //    return;
                        //}
                        if (Ex == null)
                        {
                            var bytes = e.Result;
                            File.WriteAllBytes(localPath, bytes);
                            // ImgView.Image = UIImage.FromFile(localPath);
                            // ImgView.Image = UIImage.FromFile("Images/logo.png");
                        }

                    };
                    ImgView.Image = UIImage.FromFile("Images/DefultImg.png");
                }
            }
            catch
            {
                ImgView.Image = UIImage.FromFile("Images/DefultImg.png");
            }
        }
    }
}