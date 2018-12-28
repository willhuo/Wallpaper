using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WallPaper.BusinessLogic;
using WallPaper.Datastruct;
using WallPaper.DTO;
using WallPaper.Utility;

namespace WallPaper
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText txtUsername;
        private EditText txtPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            // Create your application here
            InitUI();
        }

        private void InitUI()
        {
            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if(string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "用户名或密码为空", ToastLength.Long).Show();
                return;
            }
            
            string msg = string.Empty;
            Task.Run(() =>
            {
                var res = HttpHelper.Default.Login(username, password);                
                msg = res.Item2;
                SystemInfo.IsLogin = res.Item1;
                SystemInfo.UserView = res.Item3;
                Log.Info("WallPaper", "用户登陆完成:" + res.Item1);
            }).ContinueWith(x => RunOnUiThread(() =>
            {
                if(SystemInfo.IsLogin)
                {
                    //写入sqlite数据库
                    var res= UserLogic.Default.SaveUserInfo(SystemInfo.UserView);
                    if(!res.Item1)
                    {
                        RunOnUiThread(() => Toast.MakeText(this, res.Item2, ToastLength.Long).Show());
                    }

                    Finish();
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                }
            }));
        }
    }
}