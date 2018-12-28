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
using WallPaper.Model;

namespace WallPaper.Utility
{
    public class SqliteHelper
    {
        /*constructor*/
        private static readonly SqliteHelper _instance = new SqliteHelper();
        private SqliteHelper()
        {

        }
        public static SqliteHelper Default
        {

            get
            {
                return _instance;
            }
        }


        /*variable*/
        private string _DBPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "wallpaper.db3");


        /*public method*/
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
    }
}