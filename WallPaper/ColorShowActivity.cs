using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WallPaper
{
    [Activity(Label = "ColorShowActivity")]
    public class ColorShowActivity : Activity
    {
        List<ColorItem> colorItems = new List<ColorItem>();
        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_colorshow);
            listView = FindViewById<ListView>(Resource.Id.listViewColorList);

            colorItems.Add(new ColorItem()
            {
                Color=Android.Graphics.Color.DarkRed,
                ColorName="Dark Red",
                Code="8B0000"
            });

            colorItems.Add(new ColorItem()
            {
                Color = Android.Graphics.Color.SlateBlue,
                ColorName = "Slate Blue",
                Code = "6A5ACD"
            });

            colorItems.Add(new ColorItem()
            {
                Color = Android.Graphics.Color.ForestGreen,
                ColorName = "Forest Green",
                Code = "228B22"
            });

            listView.Adapter = new ColorAdapter(this, colorItems);
        }
    }

    public class ColorAdapter : BaseAdapter<ColorItem>
    {
        List<ColorItem> _Items;
        Activity _Context;

        public ColorAdapter(Activity context,List<ColorItem> items) : base()
        {
            this._Context = context;
            this._Items = items;
        }

        public override ColorItem this[int position]
        {
            get
            {
                return _Items[position];
            }
        }

        public override int Count
        {
            get
            {
                return _Items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _Items[position];
            View view = convertView;
            if (view == null)
                view = _Context.LayoutInflater.Inflate(Resource.Layout.ListItem, null);
            view.FindViewById<TextView>(Resource.Id.textViewColorName).Text = item.ColorName;
            view.FindViewById<TextView>(Resource.Id.textViewColorCode).Text = item.Code;
            view.FindViewById<ImageView>(Resource.Id.imageViewColor).SetBackgroundColor(item.Color);
            return view;
        }
    }

    public class ColorItem
    {
        public string ColorName { get; set; }
        public string Code { get; set; }
        public Android.Graphics.Color Color { get; set; }
    }
}