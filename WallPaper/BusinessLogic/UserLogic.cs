using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WallPaper.DTO;
using WallPaper.Model;
using WallPaper.Utility;

namespace WallPaper.BusinessLogic
{
    public class UserLogic
    {
        /*constructor*/
        private static readonly UserLogic _instance = new UserLogic();
        private UserLogic()
        {

        }
        public static UserLogic Default
        {

            get
            {
                return _instance;
            }
        }


        /*public method*/
        public Tuple<bool,string> SaveUserInfo(UserView userView)
        {
            try
            {
                var entity = MapUserViewToUserEntity(userView);
                var res = SqliteHelper.Default.SaveUserInfo(entity);
                return res;
            }
            catch(Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        public Tuple<bool,string> DeleteUserInfo()
        {
            var res = SqliteHelper.Default.DeleteUserInfo();
            return res;
        }
        public Tuple<bool,string,UserView> GetUserInfo()
        {
            try
            {
                var res = SqliteHelper.Default.GetUserInfo();
                if(res.Item1)
                {
                    var userView = MapUserEntityToUserView(res.Item3);
                    return new Tuple<bool, string, UserView>(true, null, userView);
                }
                else
                {
                    return new Tuple<bool, string, UserView>(false, res.Item2, null);
                }
            }
            catch(Exception ex)
            {
                return new Tuple<bool, string, UserView>(false, ex.Message, null);
            }
        }


        /*private method*/
        private UserEntity MapUserViewToUserEntity(UserView userView)
        {
            var entity = new UserEntity();
            entity.UserId = userView.UserId;
            entity.Username = userView.Username;
            entity.HeadPhotoUrl = userView.HeadPhotoUrl;
            entity.Name = userView.Name;
            entity.Score = userView.Score;
            return entity;
        }
        private UserView MapUserEntityToUserView(UserEntity userEntity)
        {
            UserView view = new UserView();
            view.UserId = userEntity.UserId;
            view.Username = userEntity.Username;
            view.HeadPhotoUrl = userEntity.HeadPhotoUrl;
            view.Name = userEntity.Name;
            view.Score = userEntity.Score;
            return view;
        }
    }
}