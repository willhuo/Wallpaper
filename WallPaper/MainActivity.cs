using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;
using System.Threading.Tasks;
using WallPaper.BusinessLogic;
using WallPaper.Datastruct;
using WallPaper.DTO;
using WallPaper.Utility;

namespace WallPaper
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private View _ViewWallpaper;
        private View _ViewWallpaperManager;
        private View _ViewSelf;
        private View _ViewSelfLogin;
        private ViewPager _ViewPagerHome;
        private List<View> _ViewList = new List<View>();
        private ImageView _ImageViewWallpaper;
        private TextView _LabWallpaperName;
        private TextView _LabWallpaperPixel;
        private TextView _LabWallpaperSize;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            CheckLogin();
            InitUI();
        }

        protected override void OnResume()
        {
            if(SystemInfo.IsLogin&&!SystemInfo.ShowAuthorizationView)
            {
                //子页初始化--个人主页登陆
                InitSelfLoginUI(_ViewSelfLogin);

                //全局view队列更新
                _ViewList.RemoveAt(2);
                _ViewList.Add(_ViewSelfLogin);

                //适配器view队列更新
                HomePagerAdapter adapter= (HomePagerAdapter)_ViewPagerHome.Adapter;
                adapter.UpdateViewPageItem(_ViewSelfLogin, 2);

                //重新刷新登陆页面
                _ViewPagerHome.CurrentItem = 0;

                SystemInfo.ShowAuthorizationView = true;
            }
            else if(!SystemInfo.IsLogin&&SystemInfo.ShowAuthorizationView)
            {
                //子页初始化--个人主页
                InitSeflUI(_ViewSelf);

                //全局view队列更新
                _ViewList.RemoveAt(2);
                _ViewList.Add(_ViewSelf);

                //适配器view队列更新
                HomePagerAdapter adapter = (HomePagerAdapter)_ViewPagerHome.Adapter;
                adapter.UpdateViewPageItem(_ViewSelf, 2);               

                //重新刷新登陆页面
                _ViewPagerHome.CurrentItem = 0;

                SystemInfo.ShowAuthorizationView = false;
            }
            base.OnResume();               
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    _ViewPagerHome.CurrentItem = 0;
                    return true;
                case Resource.Id.navigation_dashboard:
                    _ViewPagerHome.CurrentItem = 1;
                    return true;
                case Resource.Id.navigation_notifications:
                    _ViewPagerHome.CurrentItem = 2;
                    return true;
            }
            return false;
        }

        private void CheckLogin()
        {
            var res = UserLogic.Default.GetUserInfo();
            if(res.Item1)
            {
                SystemInfo.UserView = res.Item3;
                SystemInfo.IsLogin = true;
            }
        }

        private void InitUI()
        {
            InitViewpagerAndSubpage();

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
            
        }

        private void InitViewpagerAndSubpage()
        {
            _ViewWallpaper = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper, null);
            _ViewWallpaperManager = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper_manager, null);
            _ViewSelf = LayoutInflater.Inflate(Resource.Layout.activity_self, null);
            _ViewSelfLogin = LayoutInflater.Inflate(Resource.Layout.activity_self_login, null);

            _ViewList.Add(_ViewWallpaper);
            _ViewList.Add(_ViewWallpaperManager);
            if(SystemInfo.IsLogin)
            {
                _ViewList.Add(_ViewSelfLogin);
                SystemInfo.ShowAuthorizationView = true;
            }
            else
            {
                _ViewList.Add(_ViewSelf);
                SystemInfo.ShowAuthorizationView = false;
            }

            _ViewPagerHome = FindViewById<ViewPager>(Resource.Id.viewPagerHome);
            _ViewPagerHome.Adapter = new HomePagerAdapter(this, _ViewList);
            _ViewPagerHome.PageSelected += _ViewPagerHome_PageSelected;

            //子页初始化--主页
            InitWallpaperUI(_ViewWallpaper);

            if(SystemInfo.IsLogin)
            {
                //子页初始化--个人主页登陆
                InitSelfLoginUI(_ViewSelfLogin);
            }
            else
            {
                //子页初始化--个人主页未登录
                InitSeflUI(_ViewSelf);
            }
        }

        private void InitWallpaperUI(View view)
        {
            _ImageViewWallpaper = view.FindViewById<ImageView>(Resource.Id.imageViewWallpaper);
            Button btnPageUp = view.FindViewById<Button>(Resource.Id.btnPageUp);
            Button btnPageDown = view.FindViewById<Button>(Resource.Id.btnPageDown);
            Button btnDownloadWallpaper = view.FindViewById<Button>(Resource.Id.btnDownloadWallpaper);
            _LabWallpaperName = view.FindViewById<TextView>(Resource.Id.labWallpaperName);
            _LabWallpaperPixel = view.FindViewById<TextView>(Resource.Id.labWallpaperPixel);
            _LabWallpaperSize = view.FindViewById<TextView>(Resource.Id.labWallpaperSize);
            _ImageViewWallpaper.Click += _ImageViewWallpaper_Click;
            btnPageUp.Click += BtnPageUp_Click;
            btnPageDown.Click += BtnPageDown_Click;
            btnDownloadWallpaper.Click += BtnDownloadWallpaper_Click;
        }

        private void InitSeflUI(View view)
        {
            Button btnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;
        }

        private void InitSelfLoginUI(View view)
        {
            TextView txtNickname = view.FindViewById<TextView>(Resource.Id.txtNickname);
            TextView txtScore = view.FindViewById<TextView>(Resource.Id.txtScore);
            Button btnSetting = view.FindViewById<Button>(Resource.Id.btnSetting);

            txtNickname.Text = "昵称："+SystemInfo.UserView?.Username;
            txtScore.Text = "积分："+SystemInfo.UserView?.Score;
            btnSetting.Click += BtnSetting_Click;
        }

        private void _ViewPagerHome_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {       
            Android.Util.Log.Info("WallPaper", e.Position + "被选择了");
        }

        private void _ImageViewWallpaper_Click(object sender, System.EventArgs e)
        {
            if(SystemInfo.Image!=null)
            {
                StartActivity(new Intent(this, typeof(FullscreenWallpaperActivity)));
            }
            else
            {
                Toast.MakeText(this, "当前没有图片缓存", ToastLength.Long).Show();
            }
        }

        private void BtnPageUp_Click(object sender, System.EventArgs e)
        {
            bool flag = false;
            string msg = string.Empty;

            Task.Run(() =>
            {
                var res = HttpHelper.Default.GetNextImageInfo(0);
                flag = res.Item1;
                msg = res.Item2;
                SystemInfo.WallpaperView = res.Item3;
                Log.Info("WallPaper", "图片信息获取完成:" + flag);
            })
            .ContinueWith(x =>
            {
                if (flag)
                {
                    var res = HttpHelper.Default.GetNextImage(SystemInfo.WallpaperView.ImageUrl);
                    flag = res.Item1;
                    msg = res.Item2;
                    SystemInfo.Image = res.Item3;
                    Log.Info("WallPaper", "图片获取完成:" + flag);
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                }
            }).ContinueWith(x =>
            {
                if (flag)
                {
                    RunOnUiThread(() =>
                    {
                        _ImageViewWallpaper.SetImageBitmap(SystemInfo.Image);
                        _LabWallpaperName.Text = "图像名称：" + SystemInfo.WallpaperView.Code;
                        _LabWallpaperPixel.Text = "图像像素：800x600";
                        _LabWallpaperSize.Text = "图像大小：" + SystemInfo.WallpaperView.OriginalSize;
                        Log.Info("WallPaper", "显示图片：" + SystemInfo.WallpaperView.WallPaperId);
                    });
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                }
            });
        }

        private void BtnPageDown_Click(object sender, System.EventArgs e)
        {
            Android.Util.Log.Info("WallPaper", "点击下一页");
            Toast.MakeText(this, "点击下一页", ToastLength.Long).Show();

            bool flag = false;
            string msg = string.Empty;

            Task.Run(() =>
            {
                var res = HttpHelper.Default.GetNextImageInfo(1);
                flag = res.Item1;
                msg = res.Item2;
                SystemInfo.WallpaperView = res.Item3;
                Log.Info("WallPaper", "图片信息获取完成:" + flag);
            })
            .ContinueWith(x =>
            {
                if (flag)
                {
                    var res = HttpHelper.Default.GetNextImage(SystemInfo.WallpaperView.ImageUrl_800x600);
                    flag = res.Item1;
                    msg = res.Item2;
                    SystemInfo.Image = res.Item3;
                    Log.Info("WallPaper", "图片获取完成:" + flag);
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                }
            }).ContinueWith(x =>
            {
                if (flag)
                {
                    RunOnUiThread(() =>
                    {
                        _ImageViewWallpaper.SetImageBitmap(SystemInfo.Image);
                        _LabWallpaperName.Text = "图像名称："+ SystemInfo.WallpaperView.Code;
                        _LabWallpaperPixel.Text = "图像像素：800x600";
                        _LabWallpaperSize.Text = "图像大小：" + SystemInfo.WallpaperView.OriginalSize;
                        Log.Info("WallPaper", "显示图片：" + SystemInfo.WallpaperView.WallPaperId);
                    });
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                }
            });
        }

        private void BtnDownloadWallpaper_Click(object sender, System.EventArgs e)
        {
            Android.Util.Log.Info("WallPaper", "图片下载按钮被点击了");
            Toast.MakeText(this, "图片正在下载中", ToastLength.Long).Show();
            
        }

        private void BtnLogin_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(LoginActivity)));
        }

        private void BtnSetting_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(SettingActivity)));
        }
    }

    public class HomePagerAdapter : PagerAdapter
    {
        private Context _Context;
        private List<View> _ViewList;
        
        public HomePagerAdapter(Context context, List<View> viewList)
        {
            this._Context = context;
            this._ViewList = viewList;            
        }

        public override int Count
        {
            get
            {
                return _ViewList.Count;
            }
        }

        public override bool IsViewFromObject(View view, Object @object)
        {
            return view == @object;
        }

        public override void DestroyItem(ViewGroup container, int position, Object @object)
        {
            var view = _ViewList[position];
            container.RemoveView(view);
        }

        public override Object InstantiateItem(ViewGroup container, int position)
        {
            var view = _ViewList[position];
            container.AddView(view);
            return view;
        }

        public void UpdateViewPageItem(View view,int index)
        {
            _ViewList.RemoveAt(index);
            _ViewList.Insert(index, view);

        }
    }
}

