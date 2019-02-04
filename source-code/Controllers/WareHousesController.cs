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
            if (search != null)
            {
                listwarehouse = listwarehouse.Where(x => x.STOCKCODE.ToLower().Contains(search.ToLower()) || x.STOCKNAME.ToLower().Contains(search.ToLower())).ToList();
            }

            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listwarehouse.Count;
            listwarehouse = listwarehouse.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listwarehouse.Count;

            return PartialView(listwarehouse.Select(x => new AllModel { tblStockCod = x }).ToList());
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

                var stoclF = new stockcod
                {
                    CodeLanguage = model.tblStockCodArray[0].CodeLanguage.ToLower(),
                    STOCKNAME = model.tblStockCodArray[0].STOCKNAME,
                    STOCKCODE = model.tblStockCodArray[0].STOCKCODE,
                    CREATED = DateTime.Now,
                    LASTCHANGE = DateTime.Now

                };
                db.stockcods.Add(stoclF);
                db.SaveChanges();
                var updateItem = db.stockcods.Find(stoclF.STOCKNO);
                updateItem.IdCurrentItem = stoclF.STOCKNO;
                db.Entry(updateItem).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in model.tblStockCodArray.Skip(1).ToList())
                {

                    var tblStore = new stockcod
                    {
                        CodeLanguage = item.CodeLanguage.ToLower(),
                        IdCurrentItem = stoclF.IdCurrentItem,
                        STOCKNAME = item.STOCKNAME,
                        STOCKCODE = item.STOCKCODE,
                        CREATED = stoclF.CREATED,
                        LASTCHANGE = stoclF.LASTCHANGE


                    };

                    db.stockcods.Add(tblStore);
                }


                db.SaveChanges();


            }

            return RedirectToAction("Index");

        }

        public ActionResult Edit(int code)
        {
            var proMaster = db.stockcods.FirstOrDefault(x => x.STOCKNO == code);

            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.stockcods.FirstOrDefault(x => x.IdCurrentItem == code && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var tblItem = new stockcod
                        {
                            CREATED = proMaster.CREATED,
                            LASTCHANGE = proMaster.LASTCHANGE,
                            STOCKCODE = proMaster.STOCKCODE,
                            STOCKNAME = proMaster.STOCKNAME,
                            IdCurrentItem = proMaster.IdCurrentItem,
                            CodeLanguage = itemLang.language.ToLower()
                        };


                        db.stockcods.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var item = db.stockcods.ToList().Where(x => x.IdCurrentItem == code).ToList();
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