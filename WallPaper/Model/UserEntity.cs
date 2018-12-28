using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace WallPaper.Model
{    
    public class UserEntity
    {
        [DisplayName("主键ID")]
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [DisplayName("用户ID")]
        public int UserId { get; set; }

        [DisplayName("用户名")]
        public string Username { get; set; }

        [DisplayName("头像图片地址")]
        public string HeadPhotoUrl { get; set; }

        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("积分")]
        public int Score { get; set; }
    }
}