using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
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
using WallPaper.Adapters;
using WallPaper.BusinessLogic;
using WallPaper.Datastruct;
using WallPaper.DTO;
using WallPaper.Utility;

namespace WallPaper
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        /*variable*/
        private View _ViewWallpaper;
        private View _ViewSelf;
        private View _ViewSelfLogin;
        private ViewPager _ViewPagerHome;
        private List<View> _ViewList = new List<View>();
        private ImageView _ImageViewWallpaper;
        private TextView _TxtScore;
        private float _BeginY = 0;


        /*override method*/
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Init();                                        
        }
        protected override void OnResume()
        {
            if (SystemInfo.IsLogin && !SystemInfo.ShowAuthorizationView)
            {
                ChangeLoginPage();
            }
            else if (!SystemInfo.IsLogin && SystemInfo.ShowAuthorizationView)
            {
                ChangeNoLoginPage();
            }
            base.OnResume();
        }
        protected override void OnDestroy()
        {
            WallpaperLogic.Default.SaveBrowserWallpaperId(SystemInfo.WallpaperId);
            base.OnDestroy();
        }       


        /*component event*/
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    {
                        _ViewPagerHome.CurrentItem = 0;
                        return true;
                    }
                //case Resource.Id.navigation_dashboard:
                //    _ViewPagerHome.CurrentItem = 1;
                //    return true;
                case Resource.Id.navigation_notifications:
                    {
                        _ViewPagerHome.CurrentItem = 1;
                        return true;
                    }
            }
            return false;
        }
        private void _ViewPagerHome_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            Android.Util.Log.Info("WallPaper", e.Position + "被选择了");
        }
        private void ImageViewFullscreen_Touch(object sender, View.TouchEventArgs e)
        {
            VerticalSlideCheck(e);
        }
        private void BtnPageUp_Click(object sender, System.EventArgs e)
        {
            Android.Util.Log.Info("WallPaper", "点击上一页");
            //Toast.MakeText(this, "点击上一页", ToastLength.Long).Show();

            LoadImage(0);
        }
        private void BtnPageDown_Click(object sender, System.EventArgs e)
        {
            Android.Util.Log.Info("WallPaper", "点击下一页");
            //Toast.MakeText(this, "点击下一页", ToastLength.Long).Show();

            LoadImage(1);
        }
        private void BtnDownloadWallpaper_Click(object sender, System.EventArgs e)
        {
            DownloadWallpaper();
        }
        private void BtnLogin_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(LoginActivity)));
        }
        private void BtnSetting_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(this, typeof(SettingActivity)));
        }


        /*private method*/
        private void Init()
        {
            CheckLogin();
            LoadWallpaperBrowserTrace();
            GetAdsFromServer();
            RandomHelper.Default.ResetRandomCount();
            InitUI();
        }
        private void CheckLogin()
        {
            var res = UserLogic.Default.GetUserInfo();
            if (res.Item1)
            {
                SystemInfo.UserView = res.Item3;
                SystemInfo.IsLogin = true;
            }
        }
        private void LoadWallpaperBrowserTrace()
        {
            SystemInfo.WallpaperId = WallpaperLogic.Default.GetBrowserWallpaperId();
        }
        private void GetAdsFromServer()
        {
            Task.Run(() =>
            {
                var resAds = HttpHelper.Default.GetAdvertisements();
                if (resAds.Item1)
                {
                    Log.Info("Wallpaper", $"广告信息获取成完成，个数为：{resAds.Item3.Count}");
                    var resSave = ADLogic.Default.Save(resAds.Item3);
                    if (resSave.Item1)
                    {
                        Log.Info("Wallpaper", "广告信息DB存储完成");
                    }
                    else
                    {
                        Log.Info("Wallpaper", "广告信息DB存储异常" + resSave.Item2);
                    }
                }
                else
                {
                    Log.Info("Wallpaper", resAds.Item2);
                }
            });
        }
        private void InitUI()
        {
            InitViewpagerAndSubpage();

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        private void InitViewpagerAndSubpage()
        {
            //获取各种view
            _ViewWallpaper = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper, null);
            //_ViewWallpaperManager = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper_manager, null);
            _ViewSelf = LayoutInflater.Inflate(Resource.Layout.activity_self, null);
            _ViewSelfLogin = LayoutInflater.Inflate(Resource.Layout.activity_self_login, null);

            _ViewList.Add(_ViewWallpaper);
            //_ViewList.Add(_ViewWallpaperManager);
            if (SystemInfo.IsLogin)
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

            if (SystemInfo.IsLogin)
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

            _ImageViewWallpaper.Touch += ImageViewFullscreen_Touch;
            btnPageUp.Click += BtnPageUp_Click;
            btnPageDown.Click += BtnPageDown_Click;
            btnDownloadWallpaper.Click += BtnDownloadWallpaper_Click;

            LoadImage(1);
        }
        private void InitSeflUI(View view)
        {
            Button btnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;
        }
        private void InitSelfLoginUI(View view)
        {
            TextView txtNickname = view.FindViewById<TextView>(Resource.Id.txtNickname);
            _TxtScore = view.FindViewById<TextView>(Resource.Id.txtScore);
            Button btnSetting = view.FindViewById<Button>(Resource.Id.btnSetting);

            txtNickname.Text = "昵称：" + SystemInfo.UserView?.Username;
            _TxtScore.Text = "积分：" + SystemInfo.UserView?.Score;
            btnSetting.Click += BtnSetting_Click;
        }
        private void ChangeLoginPage()
        {
            //子页初始化--个人主页登陆
            InitSelfLoginUI(_ViewSelfLogin);

            //全局view队列更新
            _ViewList.RemoveAt(1);
            _ViewList.Add(_ViewSelfLogin);

            //适配器view队列更新
            HomePagerAdapter adapter = (HomePagerAdapter)_ViewPagerHome.Adapter;
            adapter.UpdateViewPageItem(_ViewSelfLogin, 1);

            //重新刷新登陆页面
            _ViewPagerHome.CurrentItem = 0;            

            SystemInfo.ShowAuthorizationView = true;
        }
        private void ChangeNoLoginPage()
        {
            //子页初始化--个人主页
            InitSeflUI(_ViewSelf);

            //全局view队列更新
            _ViewList.RemoveAt(1);
            _ViewList.Add(_ViewSelf);

            //适配器view队列更新
            HomePagerAdapter adapter = (HomePagerAdapter)_ViewPagerHome.Adapter;
            adapter.UpdateViewPageItem(_ViewSelf, 1);

            //重新刷新登陆页面
            _ViewPagerHome.CurrentItem = 0;

            SystemInfo.ShowAuthorizationView = false;
        }
        private void FullscreenImageJump()
        {
            //取消了点击图片，跳转到单一全屏的机制
            if (SystemInfo.Image != null)
            {
                StartActivity(new Intent(this, typeof(FullscreenWallpaperActivity)));
            }
            else
            {
                Toast.MakeText(this, "当前没有图片缓存", ToastLength.Long).Show();
            }
        }
        private void VerticalSlideCheck(View.TouchEventArgs e)
        {
            try
            {
                float currentY = 0;
                int activePointerId = 0;

                switch (e.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Down:
                        {
                            _BeginY = e.Event.GetY();
                            activePointerId = e.Event.GetPointerId(0);
                            Log.Info("WallPaper", $"触摸开始,beginY={_BeginY},pointerId={activePointerId}");
                        }
                        break;
                    case MotionEventActions.Move:
                        {
                            int pointerId = e.Event.FindPointerIndex(activePointerId);
                            currentY = e.Event.GetY(pointerId);
                            //Log.Info("WallPaper", $"触摸移动，currentX={currentX},pointerId={pointerId}");
                        }
                        break;
                    case MotionEventActions.Up:
                        {
                            int pointerId = e.Event.FindPointerIndex(activePointerId);
                            currentY = e.Event.GetX(pointerId);
                            Log.Info("WallPaper", $"触摸结束，currentY={currentY},pointerId={pointerId}");

                            var sp = currentY - _BeginY;
                            var spAbs = Math.Abs(sp);
                           
                            if(SystemInfo.ShowAD)
                            {
                                ADJump();
                                SystemInfo.ShowAD = false;
                            }
                            else
                            {
                                if (sp > 0 && spAbs > 20)
                                {
                                    LoadImage(0);
                                    Log.Info("WallPaper", $"上滑,sp={sp},spAbs={spAbs}");
                                }
                                else if (sp < 0 && spAbs > 20)
                                {
                                    LoadImage(1);
                                    Log.Info("WallPaper", $"下滑,sp={sp},spAbs={spAbs}");
                                }
                            }
                        }
                        break;
                    default:
                        Log.Info("WallPaper", "No Touch");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }
        }
        private void ADJump()
        {
            //如果遇到广告图片，点击后，启动默认浏览器跳转访问
            if (SystemInfo.ShowAD)
            {
                Intent intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse(SystemInfo.ADJumpUrl));
                StartActivity(intent);
            }
        }
        private void DownloadWallpaper()
        {
            try
            {
                //下载前检测
                if (!BeforeDownloadWallpaperCheck())
                    return;

                //设置写以后，读的权限也是默认就有了
                //bool isReadonly = Environment.MediaMountedReadOnly.Equals(Environment.ExternalStorageState);
                //bool isWriteable = Environment.MediaMounted.Equals(Environment.ExternalStorageState);
                //Log.Info("Wallpaper", $"权限检测：{isReadonly}/{isWriteable}");

                Log.Info("WallPaper", "图片下载中");

                Task.Run(() =>
                {
                    var res = HttpHelper.Default.GetNextImage(SystemInfo.WallpaperView.ImageUrl);
                    if (res.Item1)
                    {
                        //webapi扣除积分
                        var resDeductScore = HttpHelper.Default.DeductScore(SystemInfo.UserView.UserId);

                        //更新当前的扣除积分后的显示
                        RefreshSelfUI();

                        //检测并创建图片保存文件夹
                        var fileDir = CreateWallpaperFolder();

                        //保存图片到本地
                        SaveWallpaper(fileDir);
                    }
                    else
                    {
                        RunOnUiThread(() => Toast.MakeText(this, res.Item2, ToastLength.Long).Show());
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error("WallPaper", ex.ToString());
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
        private bool BeforeDownloadWallpaperCheck()
        {
            if (!SystemInfo.IsLogin)
            {
                Log.Info("WallPaper", "请登录后下载");
                Toast.MakeText(this, "请登录后下载", ToastLength.Long).Show();
                return false;
            }
            else if (SystemInfo.Image == null)
            {
                Log.Info("WallPaper", "图片为空");
                Toast.MakeText(this, "图片为空", ToastLength.Long).Show();
                return false;
            }
            return true;
        }
        private void RefreshSelfUI()
        {
            SystemInfo.UserView.Score--;
            RunOnUiThread(() => { _TxtScore.Text = "积分：" + SystemInfo.UserView?.Score; });
        }
        private Java.IO.File CreateWallpaperFolder()
        {
            Java.IO.File fileDir = new Java.IO.File("/sdcard/DCIM/wallpaper/");
            if (!fileDir.Exists())
            {
                var flag = fileDir.Mkdir();
                Log.Info("Wallpaper", $"目录{fileDir.AbsolutePath}创建结果：{flag}");
            }
            else
            {
                Log.Info("Wallpaper", $"目录{fileDir.AbsolutePath}已经存在");
            }
            return fileDir;
        }
        private void SaveWallpaper(Java.IO.File fileDir)
        {
            try
            {
                Log.Info("Wallpaper", "图片保存中");

                string fileName = SystemInfo.WallpaperView.Code + ".jpg";
                Java.IO.File fileImage = new Java.IO.File(fileDir, fileName);

                //图片保存
                using (Java.IO.FileOutputStream fos = new Java.IO.FileOutputStream(fileImage, false))
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        SystemInfo.Image.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, ms);
                        fos.Write(ms.ToArray());
                    }
                }

                //日志提示
                Log.Info("Wallpaper", $"图片{fileImage.AbsolutePath}下载完成");
                RunOnUiThread(() => Toast.MakeText(this, "图片下载完成", ToastLength.Long).Show());

                //图库刷新
                RefreshImageInSystem(fileImage);
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", "图片保存异常" + ex.ToString());
            }           
        }
        private void RefreshImageInSystem(Java.IO.File imageFile)
        {
            try
            {
                Log.Info("Wallpaper", $"path={imageFile.AbsolutePath}");
                Log.Info("Wallpaper", $"name={imageFile.Name}");

                //直接插入到媒体库中的方法
                var res = MediaStore.Images.Media.InsertImage(this.ContentResolver, imageFile.AbsolutePath, imageFile.Name, "wallpaper image");                

                //更新媒体库，貌似没有生效
                Intent intent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(imageFile);
                intent.SetData(contentUri);
                SendBroadcast(intent);                

                Log.Info("Wallpaper", "媒体库刷新完成," + res);
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
            }           
        }
        private void LoadImage(int step)
        {
            if (SystemInfo.BrowserImageTimes > 0 && SystemInfo.BrowserImageTimes % RandomHelper.Default.RandomCount == 0)
            {
                //显示广告
                ShowAD();
            }
            else
            {
                //显示壁纸
                ShowWallpaper(step);
            }          
        }
        private void ShowAD()
        {
            Task.Run(() =>
            {
                var res = ADLogic.Default.Get();
                if (res.Item1)
                {
                    //更新跳转地址
                    SystemInfo.ADJumpUrl = res.Item3;

                    //下载广告图片
                    var resPic = HttpHelper.Default.GetNextImage(res.Item2);
                    if (resPic.Item1)
                    {
                        RunOnUiThread(() =>
                        {
                            _ImageViewWallpaper.SetImageBitmap(resPic.Item3);
                            Log.Info("WallPaper", "显示广告图片");
                        });
                        SystemInfo.ShowAD = true;
                    }
                    else
                    {
                        Log.Info("Wallpaper", "广告图片下载失败"+res.Item2);
                    }
                }
                else
                {
                    Log.Info("Wallpaper", "获取广告信息失败"+res.Item2);
                }

                //图片浏览累加，并重置广告图
                SystemInfo.BrowserImageTimes++;
                RandomHelper.Default.ResetRandomCount();
            });
        }
        private void ShowWallpaper(int step)
        {
            bool flag = false;
            string msg = string.Empty;

            Task.Run(() =>
            {
                //获取壁纸信息
                var res = HttpHelper.Default.GetNextImageInfo(step);
                SystemInfo.WallpaperView = res.Item3;
                Log.Info("WallPaper", "图片信息获取完成:" + flag);

                if (res.Item1)
                {
                    //壁纸下载
                    var resImage = HttpHelper.Default.GetNextImage(SystemInfo.WallpaperView.ImageUrl_800x600);
                    SystemInfo.Image = resImage.Item3;
                    Log.Info("WallPaper", "图片获取完成:" + flag);

                    if (resImage.Item1)
                    {
                        //缓存浏览痕迹
                        SaveBrowserTrace();

                        //显示壁纸
                        RunOnUiThread(() =>
                        {
                            _ImageViewWallpaper.SetImageBitmap(SystemInfo.Image);
                            Log.Info("WallPaper", "显示图片：" + SystemInfo.WallpaperView.WallPaperId);
                        });
                    }
                    else
                    {
                        RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                    }
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, res.Item2, ToastLength.Long).Show());
                }

                //与广告显示互斥
                SystemInfo.ShowAD = false;
            });
        }
        private void SaveBrowserTrace()
        {
            //缓存图片浏览计数器
            SystemInfo.BrowserImageTimes++;
            if (SystemInfo.BrowserImageTimes % 10 == 0)
            {
                WallpaperLogic.Default.SaveBrowserWallpaperId(SystemInfo.WallpaperId);
            }
        }
    }
}