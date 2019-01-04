using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using WallPaper.Datastruct;
using WallPaper.Model;

namespace WallPaper.Utility
{
    public class SqliteHelper
    {
        /*constructor*/
        private static readonly SqliteHelper _instance = new SqliteHelper();
        private SqliteHelper()
        {
            Init();
        }
        public static SqliteHelper Default
        {

            get
            {
                return _instance;
            }
        }

        /*variable*/
        private string _DBPath { get; set; }

        /*private method*/
        private void Init()
        {
            _DBPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "wallpaper.db3");
        }

        /*用户信息管理*/
        public Tuple<bool,string> SaveUserInfo(UserEntity user)
        {
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(_DBPath))
                {
                    db.CreateTable<UserEntity>();
                    var res= db.Insert(user);
                    if(res>0)
                    {
                        return new Tuple<bool, string>(true, null);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "用户信息保存失败");
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        public Tuple<bool,string> DeleteUserInfo()
        {
            try
            {
                using(var db =new SQLiteConnection(_DBPath))
                {
                    var res= db.DeleteAll<UserEntity>();
                    if (res > 0)
                    {
                        return new Tuple<bool, string>(true, null);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "用户退出失败");
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        public Tuple<bool,string,UserEntity> GetUserInfo()
        {
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(_DBPath))
                {
                    var userList = db.Table<UserEntity>();
                    var user = userList.FirstOrDefault();
                    return new Tuple<bool, string, UserEntity>(true, null, user);
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string, UserEntity>(false, ex.Message, null);
            }
        }

        /*壁纸信息管理*/
        public void SaveBrowserWallpaperId(BrowseTraceEntity browserTrace)
        {
            try
            {
                using (var db = new SQLiteConnection(_DBPath))
                {
                    db.CreateTable<BrowseTraceEntity>();
                    var res = db.InsertOrReplace(browserTrace, typeof(BrowseTraceEntity));
                    Log.Info("Wallpaper", $"图片浏览痕迹插入或更新结果：{res}");
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }
        }
        public int GetBrowserWallpaperId()
        {
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(_DBPath))
                {
                    var browerTrace = db.Table<BrowseTraceEntity>();
                    var wallpaperId = browerTrace.FirstOrDefault()?.WallPaperId??0;
                    return wallpaperId;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return 0;
            }
        }
        public int GetWallpaperInfo()
        {
            return 0;
        }
        public void SaveWallpaperInfo(WallpaperEntity wallpaper)
        {
            try
            {
                using (var db = new SQLiteConnection(_DBPath))
                {
                    db.CreateTable<WallpaperEntity>();
                    db.Insert(wallpaper);
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }
        }

        /*广告管理*/
        public Tuple<bool, string> SaveADs(List<ADEntity> ads)
        {
            try
            {
                using (var db = new SQLiteConnection(_DBPath))
                {                    
                    db.CreateTable<ADEntity>();
                    db.DeleteAll<ADEntity>();
                    var res= db.InsertAll(ads);
                    if (res > 0)
                    {
                        return new Tuple<bool, string>(true, null);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "广告db存储失败");
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        public Tuple<bool, string, string> GetNextAD()
        {
            try
            {
                using (var db = new SQLiteConnection(_DBPath))
                {
                    var q = db.Table<ADEntity>().AsQueryable();
                    q = q.OrderBy(x => x.Id);
                    var count = q.Count();
                    if (count == 0)
                    {
                        return new Tuple<bool, string, string>(false, "广告队列为空", "");
                    }
                    var entity = q.Where(x => x.Id > SystemInfo.ADIndex).FirstOrDefault();
                    if (entity == null)
                    {
                        entity = q.FirstOrDefault();
                    }
                    SystemInfo.ADIndex = entity.Id;
                    return new Tuple<bool, string, string>(true, entity.ImageUrl, entity.JumpUrl);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string, string>(false, ex.Message, "");
            }
        }
    }
}