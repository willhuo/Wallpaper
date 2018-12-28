using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;
using Java.Util;
using Org.Apache.Http.Client;
using WallPaper.Datastruct;
using WallPaper.DTO;

namespace WallPaper.Utility
{
    public sealed class HttpHelper
    {
        /*constructor*/
        private static readonly HttpHelper _instance = new HttpHelper();
        private HttpHelper()
        {

        }
        public static HttpHelper Default
        {
            get
            {
                return _instance;
            }
        }


        /*variable*/

        /*public method*/
        public Tuple<bool,string, WallPaperView> GetNextImageInfo(int step)
        {
            try
            {
                var completeUrl = string.Format(SystemInfo.GetWallpaperUrl, SystemInfo.WallpaperId, step);
                Android.Util.Log.Info("WallPaper", "图片信息地址："+completeUrl);
                URL url = new URL(completeUrl);
                URLConnection connection= url.OpenConnection();
                var jores = (JsonObject)JsonObject.Load(connection.InputStream);
                if(jores["Success"].ToString()=="true")
                {
                    var view = GetWallPaperView((JsonObject)jores["Data"]);
                    view.Total = Convert.ToInt32(jores["Total"].ToString());
                    SystemInfo.WallpaperId = view.WallPaperId;

                    return new Tuple<bool, string, WallPaperView>(true, "", view);
                }
                else
                {
                    return new Tuple<bool, string, WallPaperView>(false, jores["Desc"].ToString(), null);
                }   
            }
            catch(Exception ex)
            {
                Android.Util.Log.Error("WallPaper", ex.ToString());
                return new Tuple<bool, string, WallPaperView>(false, ex.Message, null);
            }
        }
        public Tuple<bool, string, Bitmap> GetNextImage(string imageUrl)
        {
            try
            {
                URL url = new URL(SystemInfo.BaseUrl+imageUrl);
                var img = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
                return new Tuple<bool, string, Bitmap>(true, "", img);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("WallPaper", ex.ToString());
                return new Tuple<bool, string, Bitmap>(false, ex.Message, null);
            }
        }
        public Tuple<bool, string, UserView> Login(string username,string password)
        {
            try
            {
                string postStr = $"Username={username}&Password={password}";
                byte[] buff = Encoding.UTF8.GetBytes(postStr);
                URL url = new URL(SystemInfo.LoginUrl);
                HttpURLConnection httpUrlConnection = (HttpURLConnection)url.OpenConnection();
                httpUrlConnection.DoOutput = true;
                httpUrlConnection.RequestMethod = "POST";
                httpUrlConnection.Connect();
                Stream outputStream = httpUrlConnection.OutputStream;
                outputStream.Write(buff, 0, buff.Length);
                outputStream.Flush();
                outputStream.Close();

                var jores = (JsonObject)JsonObject.Load(httpUrlConnection.InputStream);
                if (jores["Success"].ToString() == "true")
                {
                    var view = GetUserView((JsonObject)jores["Data"]);
                    return new Tuple<bool, string, UserView>(true, "", view);
                }
                else
                {
                    return new Tuple<bool, string, UserView>(false, jores["Desc"].ToString(), null);
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("WallPaper", ex.ToString());
                return new Tuple<bool, string, UserView>(false, ex.Message, null);
            }
        }


        /*private method*/
        private WallPaperView GetWallPaperView(JsonObject jo)
        {
            var view = new WallPaperView();
            view.WallPaperId = Convert.ToInt32(jo["WallPaperId"].ToString());
            view.Code = jo["Code"].ToString().Replace("\"", null);
            view.Title = jo["Title"].ToString().Replace("\"",null);
            view.ImageUrl = jo["ImageUrl"].ToString().Replace("\"", null);
            view.ImageDownloadUrl = jo["ImageDownloadUrl"]?.ToString().Replace("\"", null);
            view.OriginalSize = jo["OriginalSize"].ToString().Replace("\"", null);
            view.ImageUrl_2560x1600 = jo["ImageUrl_2560x1600"].ToString().Replace("\"", null);
            view.ImageUrl_1920x1080 = jo["ImageUrl_1920x1080"].ToString().Replace("\"", null);
            view.ImageUrl_1440x900 = jo["ImageUrl_1440x900"].ToString().Replace("\"", null);
            view.ImageUrl_1024x768 = jo["ImageUrl_1024x768"].ToString().Replace("\"", null);
            view.ImageUrl_800x600 = jo["ImageUrl_800x600"].ToString().Replace("\"", null);
            view.CreateTime = DateTime.Parse(jo["CreateTime"].ToString().Replace("\"", null));
            view.UpdateTime = DateTime.Parse(jo["UpdateTime"].ToString().Replace("\"", null));
            view.IsDelete = Convert.ToBoolean(jo["IsDelete"].ToString().Replace("\"", null));
            view.Remark = jo["Remark"].ToString().Replace("\"", null);
            view.OwnerId = string.IsNullOrEmpty(jo["OwnerId"]?.ToString()) ? 0 : Convert.ToInt32(jo["OwnerId"].ToString());
            view.OwnerName = jo["OwnerName"]?.ToString().Replace("\"", null);
            return view;
        }
        private UserView GetUserView(JsonObject jo)
        {
            var view = new UserView();
            view.UserId = Convert.ToInt32(jo["UserId"].ToString());
            view.Username = jo["Username"].ToString().Replace("\"", null);
            view.Password = jo["Password"].ToString().Replace("\"", null);
            view.HeadPhotoUrl = jo["HeadPhotoUrl"]?.ToString().Replace("\"", null);
            view.Name = jo["Name"].ToString().Replace("\"", null);
            view.Email = jo["Email"]?.ToString().Replace("\"", null);
            view.Mobile = jo["Mobile"]?.ToString().Replace("\"", null);
            view.Score = Convert.ToInt32(jo["Score"].ToString());
            view.RegTime = DateTime.Parse(jo["RegTime"].ToString().Replace("\"", null));
            view.RegIP = jo["RegIP"].ToString().Replace("\"", null);
            view.AllowLogin = Convert.ToBoolean(jo["AllowLogin"].ToString().Replace("\"", null));
            view.LastLoginTime = DateTime.Parse(jo["LastLoginTime"].ToString().Replace("\"", null));
            view.LastLoginIP = jo["LastLoginIP"].ToString().Replace("\"", null);
            view.LoginTimes = Convert.ToInt32(jo["LoginTimes"].ToString());
            view.IsDelete = Convert.ToBoolean(jo["IsDelete"].ToString().Replace("\"", null));
            view.DepartmentId = Convert.ToInt32(jo["DepartmentId"].ToString());
            view.DepartmentName = jo["DepartmentName"].ToString().Replace("\"", null);
            view.RoleNames = jo["RoleNames"]?.ToString().Replace("\"", null);
            view.CreatorId = Convert.ToInt32(jo["CreatorId"].ToString());
            view.Remark = jo["Remark"]?.ToString().Replace("\"", null);
            return view;
        }
    }
}