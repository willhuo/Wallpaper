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
    public class WallPaperView
    {
        [DisplayName("壁纸ID")]
        public int WallPaperId { get; set; }

        [DisplayName("壁纸编码")]
        public string Code { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl { get; set; }

        [DisplayName("下载地址")]
        public string ImageDownloadUrl { get; set; }

        [DisplayName("壁纸原始大小")]
        public string OriginalSize { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl_2560x1600 { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl_1920x1080 { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl_1440x900 { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl_1024x768 { get; set; }

        [DisplayName("壁纸地址")]
        public string ImageUrl_800x600 { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("更新时间")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("是否删除")]
        public bool IsDelete { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        [DisplayName("所属人ID")]
        public int? OwnerId { get; set; }

        [DisplayName("所属人姓名")]
        public string OwnerName { get; set; }

        [DisplayName("图片个数")]
        public int Total { get; set; }
    }
}