using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WallPaper.Utility;

namespace WallPaper
{
    [Activity(Label = "RegActivity")]
    public class RegActivity : Activity
    {
        /*variable*/
        private EditText _TxtUsername;
        private EditText _TxtPassword;
        private EditText _TxtPassword2;

        /*override method*/
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reg);

            // Create your application here
            InitUI();
        }


        /*private method*/
        private void InitUI()
        {
            _TxtUsername = FindViewById<EditText>(Resource.Id.txtRegUsername);
            _TxtPassword = FindViewById<EditText>(Resource.Id.txtRegPassword);
            _TxtPassword2 = FindViewById<EditText>(Resource.Id.txtRegPassword2);
            Button btnReg = FindViewById<Button>(Resource.Id.btnRegConfirm);

            btnReg.Click += BtnReg_Click;
        }
        private void Register(string username, string password)
        {
            var res = HttpHelper.Default.Register(username, password);
            if (res.Item1)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, res.Item2, ToastLength.Long).Show();
                });
                Task.Delay(1000).Wait();
                StartActivity(new Intent(this, typeof(LoginActivity)));
                this.Finish();
            }
            else
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, res.Item2, ToastLength.Long).Show();
                });
            }
        }


        /*component event*/
        private void BtnReg_Click(object sender, EventArgs e)
        {
            var username = _TxtUsername.Text;
            var password = _TxtPassword.Text;
            var password2 = _TxtPassword2.Text;

            if(string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password)||string.IsNullOrEmpty(password2))
            {
                Toast.MakeText(this, "用户名或密码为空", ToastLength.Long).Show();
                return;
            }
            else if(password!=password2)
            {
                Toast.MakeText(this, "请输入相同的密码", ToastLength.Long).Show();
                return;
            }

            Task.Run(() => Register(username, password));
        }
    }
}