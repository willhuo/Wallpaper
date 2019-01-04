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

namespace WallPaper.DTO
{
    public class ADView
    {
        [DisplayName("广告ID")]
        public int ADId { get; set; }

        [DisplayName("广告名字")]
        public string ADName { get; set; }

        [DisplayName("广告图片")]
        public string ImageUrl { get; set; }

        [DisplayName("广告地址")]
        public string JumpUrl { get; set; }

        [DisplayName("广告位置")]
        public int Position { get; set; }

        [DisplayName("是否激活")]
        public bool IsActive { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("更新时间")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        [DisplayName("广告位置")]
        public string PositionCN { get; set; }
    }
}