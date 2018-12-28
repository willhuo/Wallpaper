using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WallPaper.Datastruct;
using WallPaper.Utility;

namespace WallPaper
{
    [Activity(Label = "FullscreenWallpaperActivity")]
    public class FullscreenWallpaperActivity : Activity
    {
        private float _BeginX = 0;
        private ImageView _ImageViewFullscreen;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_fullscreen_wallpaper);

            // Create your application here
            InitUI();
        }

        private void InitUI()
        {
            _ImageViewFullscreen = FindViewById<ImageView>(Resource.Id.imageViewFullscreen);
            _ImageViewFullscreen.SetImageBitmap(SystemInfo.Image);
            _ImageViewFullscreen.Touch += ImageViewFullscreen_Touch;
        }

        private void ImageViewFullscreen_Touch(object sender, View.TouchEventArgs e)
        {
            try
            {
                float currentX = 0;
                int activePointerId = 0;
                switch (e.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        {
                            _BeginX = e.Event.GetX();
                            activePointerId = e.Event.GetPointerId(0);
                            Log.Info("WallPaper", $"触摸开始,beginX={_BeginX},pointerId={activePointerId}");
                        }
                        break;
                    case MotionEventActions.Move:
                        {
                            int pointerId = e.Event.FindPointerIndex(activePointerId);
                            currentX = e.Event.GetX(pointerId);
                            //Log.Info("WallPaper", $"触摸移动，currentX={currentX},pointerId={pointerId}");
                        }
                        break;
                    case MotionEventActions.Up:
                        {
                            int pointerId = e.Event.FindPointerIndex(activePointerId);
                            currentX = e.Event.GetX(pointerId);
                            Log.Info("WallPaper", $"触摸结束，currentX={currentX},pointerId={pointerId}");

                            if ((currentX - _BeginX) > 0 && Math.Abs(currentX - _BeginX) > 20)
                            {
                                GetNextImage(0);
                                Log.Info("WallPaper", "右滑");
                            }
                            else if ((currentX - _BeginX) < 0 && Math.Abs(currentX - _BeginX) > 20)
                            {
                                GetNextImage(1);
                                Log.Info("WallPaper", "左滑");
                            }
                        }
                        break;
                    default:
                        Log.Info("WallPaper", "No Touch");
                        break;
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }            
        }

        private void GetNextImage(int step)
        {
            bool flag = false;
            string msg = string.Empty;

            Task.Run(() =>
            {
                var res = HttpHelper.Default.GetNextImageInfo(step);
                flag = res.Item1;
                msg = res.Item2;
                SystemInfo.WallpaperView = res.Item3;
                Log.Info("WallPaper", "图片信息获取完成:" + flag);
            })
           .ContinueWith(x =>
           {
               if (flag)
               {
                   var res = HttpHelper.Default.GetNextImage(SystemInfo.WallpaperView.ImageUrl_800x600);
                   flag = res.Item1;
                   msg = res.Item2;
                   SystemInfo.Image = res.Item3;
                   Log.Info("WallPaper", "图片获取完成:" + flag);
               }
               else
               {
                   RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
               }
           }).ContinueWith(x =>
           {
               if (flag)
               {
                   RunOnUiThread(() =>
                   {
                       _ImageViewFullscreen.SetImageBitmap(SystemInfo.Image);
                       Log.Info("WallPaper", "显示图片：" + SystemInfo.WallpaperView.WallPaperId);
                   });
               }
               else
               {
                   RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
               }
           });
        }
    }
}