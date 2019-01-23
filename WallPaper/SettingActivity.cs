using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
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
        /*override*/
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_setting);

            // Create your application here
            InitUI();
        }


        /*component event*/
        private void BtnApplicationExist_Click(object sender, EventArgs e)
        {
            UserLogic.Default.DeleteUserInfo();
            SystemInfo.IsLogin = false;
            Finish();
        }
        private void BtnTest_Click(object sender, EventArgs e)
        {
            PermissionTest();
        } 


        /*private method*/
        private void InitUI()
        {
            Button btnApplicationExist = FindViewById<Button>(Resource.Id.btnApplicationExist);
            Button btnTest = FindViewById<Button>(Resource.Id.btnTest);

            btnApplicationExist.Click += BtnApplicationExist_Click;
            btnTest.Click += BtnTest_Click;
        }
        private void UserInfoTest()
        {
            var res = UserLogic.Default.GetUserInfo();
            Log.Info("Wallpaper", $"用户获取结果：{res.Item1},{res.Item3?.Username}");
        }
        private void DialogTest()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertDialog = builder
                .SetTitle("升级提示")
                .SetMessage("发现新版本，是否进行升级？")
                .SetNegativeButton("取消", (s, e) => { Toast.MakeText(this, "更新已经取消", ToastLength.Long).Show(); })
                .SetPositiveButton("升级", (s, e) =>
                {
                    Toast.MakeText(this, "准备升级", ToastLength.Long).Show();

                    string url = "http://www.zhcto.com/packages/WallPaperfcd0168b-001d-4550-b8bf-6eeedb5a7c2d.apk";
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(url));
                    StartActivity(intent);
                })
                .Create();
            alertDialog.Show();
        }
        private void PermissionTest()
        {
            Utility.EnviromentHelper.Default.ApplyRuntimePermission(this, Manifest.Permission.WriteExternalStorage, 1000);
        }
    }
}