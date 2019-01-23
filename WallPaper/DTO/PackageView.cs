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
    public class PackageView
    {
        [DisplayName("主键")]
        public int PackageId { get; set; }

        [DisplayName("软件名")]
        public string Name { get; set; }

        [DisplayName("版本")]
        public string Version { get; set; }

        [DisplayName("更新描述")]
        public string Description { get; set; }

        [DisplayName("上传时间")]
        public DateTime UploadTime { get; set; }

        [DisplayName("下载次数")]
        public int Downloads { get; set; }

        [DisplayName("文件大小")]
        public long Size { get; set; }

        [DisplayName("存储路径")]
        public string Path { get; set; }

        [DisplayName("是否激活")]
        public bool IsActive { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
    }
}