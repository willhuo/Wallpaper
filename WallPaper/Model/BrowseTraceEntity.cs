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
    public class BrowseTraceEntity
    {
        [DisplayName("主键ID")]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [DisplayName("壁纸ID")]
        public int WallPaperId { get; set; }
    }
}