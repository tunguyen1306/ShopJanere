using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StockController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /stock/
        public ActionResult Index()
        {
            var list = db.stockcods.ToList();
            list.Insert(0, new stockcod { STOCKNO = 0, STOCKNAME = "Select Ware House" });
            ViewBag.ListStockCod = list;
            return View();
        }
        public ActionResult IndexAjax(string search=null,int start = 0, int view = 10, int stockcodId = 0)
        {
            //var liststock = db.stocks.Join(db.stockcods, st => st.STOCKNO, stt => stt.STOCKNO, (st, stt) => new AllModel { tblStock = st, tblStockCod = stt }).ToList();
            var listStock = (from stock in db.stocks
                            join stockcod in db.stockcods on stock.STOCKNO equals stockcod.STOCKNO
                            join item in db.items on stock.ARTNO equals item.ARTNO
                            select new AllModel {tblStock = stock,tblitem = item,tblStockCod = stockcod});
            if (stockcodId != 0)
            {
                var typeText = db.stockcods.FirstOrDefault(x => x.STOCKNO == stockcodId);
                if (typeText!=null)
                {
                    listStock = listStock.Where(x => x.tblStock.STOCKNO == typeText.STOCKNO);
                }
               
            }
            if (search!=null)
            {
                listStock = listStock.Where(x => x.tblStockCod.STOCKNAME.ToLower().Contains(search.ToLower()) || x.tblStockCod.STOCKCODE.ToLower().Contains(search.ToLower()) ||  x.tblitem.ARTCODE.ToLower().Contains(search.ToLower()) || x.tblitem.ARTNAME.ToLower().Contains(search.ToLower()));
            }
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listStock.Count();
            listStock = listStock.OrderByDescending(x=>x.tblStock.LINENO).Skip(start).Take(view);
            ViewBag.ViewOf = listStock.Count();

            return PartialView(listStock.ToList());
        }
        // GET: /stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: /stock/Create
        public ActionResult Create()
        {
            ViewBag.ListProduct = db.items.Where(x=>x.EXPORTABLE=="T").ToList();
            ViewBag.ListWareHouse = db.stockcods.ToList();

            return View(new AllModel {tblStock = new stock(),tblStockCod = new stockcod(),tblitem = new item()});
        }

        // POST: /stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(AllModel model, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                var checkStock = db.stocks.FirstOrDefault(x => x.STOCKNO == model.tblStock.STOCKNO && x.ARTNO== model.tblStock.ARTNO);
                if (checkStock!=null)
                {
                    var tblStock = db.stocks.Find(checkStock.LINENO);
                    tblStock.VOLUME = checkStock.VOLUME+ model.tblStock.VOLUME;
                    tblStock.SIZENO = model.tblStock.SIZENO;
                    db.Entry(tblStock).State=EntityState.Modified;

                }
                else
                {
                    db.stocks.Add(model.tblStock);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(model.tblStock);
        }

        // GET: /stock/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListProduct = db.items.Where(x=>x.EXPORTABLE=="T").ToList();
            ViewBag.ListWareHouse = db.stockcods.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
         
            if (stock == null)
            {
               
                return HttpNotFound();
            }
            var item= db.items.FirstOrDefault(x=>x.ARTNO== stock.ARTNO);
            var stockcods = db.stockcods.FirstOrDefault(x=>x.STOCKNO== stock.STOCKNO);
            return View(new AllModel { tblStock = stock,tblitem = item ,tblStockCod = stockcods });

        }

       
        [HttpPost]

        public ActionResult Edit(AllModel model, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(model.tblStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model.tblStock);
        }

        // GET: /stock/Delete/5
        public ActionResult Delete(int? id)
        {
            stock stock = db.stocks.Find(id);
            db.stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /stock/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    stock stock = db.stocks.Find(id);
        //    db.stocks.Remove(stock);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult _WareHouseAjaxAutoComplete(string query)
        {

           
            var list = db.stockcods.Select(x => new AllModel { tblStockCod = x }).Where(x =>  x.tblStockCod.STOCKNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblStockCod.STOCKNO, label = x.tblStockCod.STOCKNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ItemAjaxAutoComplete(string query)
        {

          
            var list = db.items.Select(x => new AllModel { tblitem = x }).Where(x => x.tblitem.ARTNAME.ToLower().Contains(query.ToLower()) && x.tblitem.EXPORTABLE == "T").ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}
