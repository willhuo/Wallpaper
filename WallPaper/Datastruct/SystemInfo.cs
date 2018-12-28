using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WallPaper.DTO;

namespace WallPaper.Datastruct
{
    public class SystemInfo
    {        
        public static string BaseUrl = "http://wallpaper.zhcto.com";
        public static string GetWallpaperUrl = "http://wallpaper.zhcto.com/wallpaper/getsingleimage?WallPaperId={0}&Step={1}";
        public static string LoginUrl = "http://wallpaper.zhcto.com/User/Login";

        public static bool IsLogin { get; set; }
        public static bool ShowAuthorizationView { get; set; }

        public static UserView UserView { get; set; }
        public static WallPaperView WallpaperView { get; set; }
        public static int WallpaperId = 0;
        public static Bitmap Image { get; set; }
    }
}