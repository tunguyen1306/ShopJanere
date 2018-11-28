using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Helper
{
    public static class DropdownList
    {
        static veebdbEntities data = new veebdbEntities();
        public static List<SelectListItem> MasterMetaGroup(int defaultSelected=0)
        {
            var getList = data.metagrups.Where(m => m.PARENTNO == 0);
            var list = new List<SelectListItem>();
            foreach (var item in getList)
            {
                list.Add(new SelectListItem() { Text = item.PARENTNO.ToString(), Value = item.METAGROUPNAME.ToString() });
            }
            if (defaultSelected != 0)
            {
                list.Where(e => e.Value == defaultSelected.ToString()).FirstOrDefault().Selected = true;
            }
            return list;
        }
        public static List<SelectListItem> MetaGroup(int defaultSelected=0)
        {
            var getList = data.metagrups.Where(m => m.PARENTNO != 0);
            var list = new List<SelectListItem>();
            foreach (var item in getList)
            {
                list.Add(new SelectListItem() { Text = item.PARENTNO.ToString(), Value = item.METAGROUPNAME.ToString() });
            }
            if (defaultSelected != 0)
            {
                list.Where(e => e.Value == defaultSelected.ToString()).FirstOrDefault().Selected = true;
            }
            return list;
        }
        public static List<SelectListItem> Catalogue(int defaultSelected = 0)
        {
            var getList = data.catalogues;
            var list = new List<SelectListItem>();
            foreach (var item in getList)
            {
                list.Add(new SelectListItem() { Text = item.CatalogueCode.ToString(), Value = item.CatalogueName.ToString() });
            }
            if (defaultSelected != 0)
            {
                list.Where(e => e.Value == defaultSelected.ToString()).FirstOrDefault().Selected = true;
            }
            return list;
        }
        public static List<SelectListItem> Category(int defaultSelected = 0)
        {
            var getList = data.categories;
            var list = new List<SelectListItem>();
            foreach (var item in getList)
            {
                list.Add(new SelectListItem() { Text = item.CATEGORYNO.ToString(), Value = item.CATEGORYNAME.ToString() });
            }
            if (defaultSelected != 0)
            {
                list.Where(e => e.Value == defaultSelected.ToString()).FirstOrDefault().Selected = true;
            }
            return list;
        }

    }
}