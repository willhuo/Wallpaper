using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;

namespace WallPaper
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private ViewPager _ViewPagerHome;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
           
            InitUI();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    _ViewPagerHome.CurrentItem = 0;
                    Toast.MakeText(this, "0被选择", ToastLength.Short).Show();
                    return true;
                case Resource.Id.navigation_dashboard:
                    _ViewPagerHome.CurrentItem = 1;
                    Toast.MakeText(this, "1被选择", ToastLength.Short).Show();
                    return true;
                case Resource.Id.navigation_notifications:
                    _ViewPagerHome.CurrentItem = 2;
                    Toast.MakeText(this, "2被选择", ToastLength.Short).Show();
                    return true;
            }
            return false;
        }

        private void InitUI()
        {
            var viewWallpaper = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper, null);
            var viewWallpaperManager = LayoutInflater.Inflate(Resource.Layout.activity_wallpaper_manager, null);
            var viewSelf = LayoutInflater.Inflate(Resource.Layout.activity_self, null);

            List<View> viewList = new List<View>();
            viewList.Add(viewWallpaper);
            viewList.Add(viewWallpaperManager);
            viewList.Add(viewSelf);

            _ViewPagerHome = FindViewById<ViewPager>(Resource.Id.viewPagerHome);
            _ViewPagerHome.Adapter = new HomePagerAdapter(this,viewList);
            _ViewPagerHome.PageSelected += _ViewPagerHome_PageSelected;
            
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        private void _ViewPagerHome_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            Android.Util.Log.Info("WallPaper", e.Position + "被选择了");
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
    }
}

