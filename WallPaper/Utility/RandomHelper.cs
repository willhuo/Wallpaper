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

namespace WallPaper.Utility
{
    public class RandomHelper
    {
        /*constructor*/
        private static readonly RandomHelper _instance = new RandomHelper();
        private RandomHelper()
        {
            Init();
        }
        public static RandomHelper Default
        {
            get
            {
                return _instance;
            }
        }


        /*variable*/
        private Random _Ra;


        /*attr*/
        public int RandomCount { get; set; } = 25;


        /*private method*/
        private void Init()
        {
            _Ra = new Random();
        }


        /*public method*/
        public void ResetRandomCount()
        {
            RandomCount = _Ra.Next(25, 40);
            //RandomCount = 5;
        }
    }
}