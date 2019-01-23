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
        //public static string BaseUrl = "http://wallpaper.zhcto.com";
        //public static string GetWallpaperUrl = "http://wallpaper.zhcto.com/wallpaper/getsingleimage?WallPaperId={0}&Step={1}";
        //public static string LoginUrl = "http://wallpaper.zhcto.com/User/Login";
        //public static string GetADsUrl = "http://wallpaper.zhcto.com/AD/GetPositionAds";
        //public static string DeductScoreUrl = "http://wallpaper.zhcto.com/User/DeductScore?UserId=";
        //public static string RegUrl = "http://wallpaper.zhcto.com/User/Register";

        public static string BaseUrl = "http://www.kayum.cn/";
        public static string GetWallpaperUrl = "http://www.kayum.cn/wallpaper/getsingleimage?WallPaperId={0}&Step={1}";
        public static string LoginUrl = "http://www.kayum.cn/User/Login";
        public static string GetADsUrl = "http://www.kayum.cn/AD/GetPositionAds";
        public static string DeductScoreUrl = "http://www.kayum.cn/User/DeductScore?UserId=";
        public static string RegUrl = "http://www.kayum.cn/User/Register";


        public static string UpdatePackageUrl = "http://www.zhcto.com/package/getupdatepackage?Name=";


        public static bool IsLogin { get; set; }
        public static bool ShowAuthorizationView { get; set; }
        public static UserView UserView { get; set; }


        public static int WallpaperId = 0;
        public static WallPaperView WallpaperView { get; set; }
        public static Bitmap Image { get; set; }
        public static int BrowserImageTimes { get; set; }


        public static int ADIndex { get; set; }
        public static bool ShowAD { get; set; }
        public static string ADJumpUrl { get; set; }

        public static PackageView PackageView { get; set; }
        public static string ProgramName = "新疆壁纸APP";
    }
}