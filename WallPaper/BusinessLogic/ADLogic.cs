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
using WallPaper.Datastruct;
using WallPaper.DTO;
using WallPaper.Model;
using WallPaper.Utility;

namespace WallPaper.BusinessLogic
{
    public class ADLogic
    {
        /*constructor*/
        private static readonly ADLogic _instance = new ADLogic();
        private ADLogic()
        {

        }
        public static ADLogic Default
        {

            get
            {
                return _instance;
            }
        }


        /*public method*/
        public Tuple<bool,string> Save(List<ADView> adViewList)
        {
            try
            {
                var list = MapViewToEntity(adViewList);
                var res = SqliteHelper.Default.SaveADs(list);
                return res;
            }
            catch(Exception ex)
            {
                Log.Error("Wallpaper", ex.ToString());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        public Tuple<bool,string,string> Get()
        {
            var res = SqliteHelper.Default.GetNextAD();
            return res;
        }


        /*private method*/
        private List<ADEntity> MapViewToEntity(List<ADView> adViewList)
        {
            var list = new List<ADEntity>();
            foreach(var v in adViewList)
            {
                var entity = new ADEntity();
                entity.ImageUrl = v.ImageUrl;
                entity.JumpUrl = v.JumpUrl;
                list.Add(entity);
            }
            return list;
        }
    }
}