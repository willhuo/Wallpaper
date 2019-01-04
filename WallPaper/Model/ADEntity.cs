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
    public class ADEntity
    {
        [DisplayName("主键ID")]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [DisplayName("广告图片")]
        public string ImageUrl { get; set; }

        [DisplayName("广告地址")]
        public string JumpUrl { get; set; }       
    }
}