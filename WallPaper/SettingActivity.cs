using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WallPaper.BusinessLogic;
using WallPaper.Datastruct;

namespace WallPaper
{
    [Activity(Label = "SettingActivity")]
    public class SettingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_setting);

            // Create your application here
            InitUI();
        }

        private void InitUI()
        {
            Button btnApplicationExist = FindViewById<Button>(Resource.Id.btnApplicationExist);
            Button btnTest = FindViewById<Button>(Resource.Id.btnTest);

            btnApplicationExist.Click += BtnApplicationExist_Click;
            btnTest.Click += BtnTest_Click;
        }

        private void BtnApplicationExist_Click(object sender, EventArgs e)
        {
            UserLogic.Default.DeleteUserInfo();
            SystemInfo.IsLogin = false;
            Finish();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            var res = UserLogic.Default.GetUserInfo();
            Log.Info("Wallpaper", $"用户获取结果：{res.Item1},{res.Item3?.Username}");
        }
    }
}