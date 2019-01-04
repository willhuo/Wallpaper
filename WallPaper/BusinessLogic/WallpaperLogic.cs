using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WallPaper.Model;
using WallPaper.Utility;

namespace WallPaper.BusinessLogic
{
    public class WallpaperLogic
    {
        /*constructor*/
        private static readonly WallpaperLogic _instance = new WallpaperLogic();
        private WallpaperLogic()
        {

        }
        public static WallpaperLogic Default
        {

            get
            {
                return _instance;
            }
        }


        /*图片浏览痕迹处理*/
        public void SaveBrowserWallpaperId(int wallpaperId)
        {
            try
            {
                var entity = new BrowseTraceEntity();
                entity.Id = 1;
                entity.WallPaperId = wallpaperId;

                SqliteHelper.Default.SaveBrowserWallpaperId(entity);
            }
            catch (Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }
        }
        public int GetBrowserWallpaperId()
        {
            try
            {
                var res = SqliteHelper.Default.GetBrowserWallpaperId();
                return res;
            }
            catch (Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return 0;
            }
        }
    }
}