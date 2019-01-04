using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;

namespace WallPaper.Adapters
{
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

        public override int GetItemPosition(Object @object)
        {
            return PositionNone;
        }

        public void UpdateViewPageItem(View view, int index)
        {
            _ViewList.RemoveAt(index);
            _ViewList.Insert(index, view);

            //刷新view缓存
            this.NotifyDataSetChanged();
        }
    }
}