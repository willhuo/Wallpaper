using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace WallPaper.Utility
{
    public class EnviromentHelper
    {
        /*constructor*/
        private static readonly EnviromentHelper _instance = new EnviromentHelper();
        private EnviromentHelper()
        {

        }
        public static EnviromentHelper Default
        {
            get
            {
                return _instance;
            }
        }


        /*public method*/
        public bool HasSdcard()
        {
            var state = Android.OS.Environment.ExternalStorageState;
            if (state == Android.OS.Environment.MediaMounted)
                return true;
            else
                return false;            
        }
        public long GetSDFreeSize()
        {
            //取得SD卡文件路径
            File path = Android.OS.Environment.ExternalStorageDirectory;
            StatFs sf = new StatFs(path.Path);
            //获取单个数据块的大小(Byte)
            long blockSize = sf.BlockSizeLong;
            //空闲的数据块的数量
            long freeBlocks = sf.AvailableBlocksLong;
            //返回SD卡空闲大小
            //return freeBlocks * blockSize;  //单位Byte
            //return (freeBlocks * blockSize)/1024;   //单位KB
            return (freeBlocks * blockSize) / 1024 / 1024; //单位MB
        }
        public long GetSDAllSize()
        {
            //取得SD卡文件路径
            File path = Android.OS.Environment.ExternalStorageDirectory;
            StatFs sf = new StatFs(path.Path);
            //获取单个数据块的大小(Byte)
            long blockSize = sf.BlockSizeLong;
            //获取所有数据块数
            long allBlocks = sf.BlockCountLong;
            //返回SD卡大小
            //return allBlocks * blockSize; //单位Byte
            //return (allBlocks * blockSize)/1024; //单位KB
            return (allBlocks * blockSize) / 1024 / 1024; //单位MB
        }
        public string GetVersion(Context context)
        {
            try
            {
                var manager = context.PackageManager;
                var info = manager.GetPackageInfo(context.PackageName, 0);
                return info.VersionCode.ToString();
            }
            catch(Exception ex)
            {
                Android.Util.Log.Error("Wallpaper", ex.ToString());
                return null;
            }
        }
        public bool ApplyRuntimePermission(Context context, string askedPermission,int requestCode)
        {
            //Manifest.Permission.WriteExternalStorage
            //requestCode=1000
            //授权检测
            if (context.CheckSelfPermission(askedPermission) == Android.Content.PM.Permission.Granted)
            {
                Android.Util.Log.Info("Wallpaper", askedPermission + "已经授权");
                return true;
            }
            else
            {
                Android.Util.Log.Info("Wallpaper", askedPermission + "未经授权");
            }

            string[] permissionArray = { askedPermission };
            Activity activity = context as Activity;
            if(activity.ShouldShowRequestPermissionRationale(askedPermission))
            {
                Snackbar.Make(activity.FindViewById(Android.Resource.Id.Content), "申请存储写入权限", Snackbar.LengthIndefinite)
                    .SetAction("OK", delegate { activity.RequestPermissions(permissionArray, requestCode); });
            }
            else
            {
                activity.RequestPermissions(permissionArray, requestCode);
            }
            return true;
        }
    }
}