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
    public class UserView
    {
        [DisplayName("用户ID")]
        public int UserId { get; set; }

        [DisplayName("用户名")]
        public string Username { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("头像图片地址")]
        public string HeadPhotoUrl { get; set; }

        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("邮箱")]
        public string Email { get; set; }

        [DisplayName("手机号码")]
        public string Mobile { get; set; }

        [DisplayName("积分")]
        public int Score { get; set; }

        [DisplayName("注册时间")]
        public DateTime RegTime { get; set; }

        [DisplayName("注册IP")]
        public string RegIP { get; set; }

        [DisplayName("允许登陆")]
        public bool AllowLogin { get; set; }

        [DisplayName("最后登陆时间")]
        public DateTime LastLoginTime { get; set; }

        [DisplayName("最后登陆IP")]
        public string LastLoginIP { get; set; }

        [DisplayName("登陆次数")]
        public int LoginTimes { get; set; }

        [DisplayName("是否删除")]
        public bool IsDelete { get; set; }

        [DisplayName("部门Id")]
        public int DepartmentId { get; set; }

        [DisplayName("部门名字")]
        public string DepartmentName { get; set; }

        [DisplayName("角色名")]
        public string RoleNames { get; set; }

        [DisplayName("创建人ID")]
        public int CreatorId { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
    }
}