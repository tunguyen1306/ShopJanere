using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WareHousesController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        // GET: WareHouses
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(string search = null, int start = 0, int view = 10)
        {
            var listwarehouse = db.stockcods.ToList();
            if (search!=null)
            {
                listwarehouse = listwarehouse.Where(x => x.STOCKCODE.ToLower().Contains(search.ToLower()) || x.STOCKNAME.ToLower().Contains(search.ToLower())).ToList();
            }
           
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listwarehouse.Count;
            listwarehouse = listwarehouse.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listwarehouse.Count;

            return PartialView(listwarehouse.Select(x=>new AllModel {tblStockCod = x}).ToList());
        }
        public ActionResult Create()
        {
            return View(new AllModel { tblStockCod = new stockcod() });
        }
        [HttpPost]
        public ActionResult Create(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblStockCodArray != null)
            {
                foreach (var item in model.tblStockCodArray)
                {
                    var tblStockCode = db.stockcods.FirstOrDefault(x => x.STOCKCODE == item.STOCKCODE && x.CodeLanguage==item.CodeLanguage);
                    if (tblStockCode==null)
                    {
                         item.CREATED = DateTime.Now;
                    item.LASTCHANGE = DateTime.Now;
                    db.stockcods.Add(item);
                    
                    }
                    else
                    {
                        model.messError = "Code Duplicate";
                        model.tblStockCod= item;
                        return View(model);
                    }
                   
                   
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        public ActionResult Edit(string code)
        {
            var proMaster = db.stockcods.FirstOrDefault(x => x.STOCKCODE == code);
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.countries.Where(x => x.status == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.stockcods.FirstOrDefault(x => x.STOCKCODE == code && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var tblItem = new stockcod();
                        tblItem.CREATED = proMaster.CREATED;
                        tblItem.LASTCHANGE = proMaster.LASTCHANGE;
                        tblItem.STOCKCODE = proMaster.STOCKCODE;
                        tblItem.STOCKNAME = proMaster.STOCKNAME;
                       
                       
                        tblItem.CodeLanguage = itemLang.language.ToLower();
                        db.stockcods.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var item = db.stockcods.ToList().Where(x => x.STOCKCODE.ToLower() == code.ToLower()).ToList();
            return View(new AllModel { listStockCod = item, tblStockCod = proMaster });
        }
        [HttpPost]
        public ActionResult Edit(AllModel model, HttpPostedFileBase[] inputfile)
        {

            if (model.tblStockCodArray != null)
            {
                foreach (var item in model.tblStockCodArray)
                {
                    var tem = db.stockcods.Find(item.STOCKNO);

                 
                    tem.STOCKCODE = item.STOCKCODE;
                    tem.STOCKNAME = item.STOCKNAME;
                    tem.CREATED = item.CREATED;
                    tem.LASTCHANGE = item.LASTCHANGE;
                    tem.CodeLanguage = item.CodeLanguage.ToLower();
                    db.Entry(tem).State = EntityState.Modified;
                    

                }

            }

            db.SaveChanges();


            return RedirectToAction("Index");


        }





 
       

    }
}