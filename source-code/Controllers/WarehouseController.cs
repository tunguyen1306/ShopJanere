using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class WarehouseController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /warehouse/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listwarehouse = db.warehouses.ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listwarehouse.Count;
            listwarehouse = listwarehouse.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listwarehouse.Count;

            return PartialView(listwarehouse);
        }
        // GET: /warehouse/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            warehouse warehouse = db.warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: /warehouse/Create
        public ActionResult Create()
        {
            return View(new warehouse());
        }

        // POST: /warehouse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.warehouses.Add(warehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(warehouse);
        }

        // GET: /warehouse/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            warehouse warehouse = db.warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: /warehouse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        // GET: /warehouse/Delete/5
        public ActionResult Delete(int? id)
        {
            warehouse warehouse = db.warehouses.Find(id);
            db.warehouses.Remove(warehouse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /warehouse/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    warehouse warehouse = db.warehouses.Find(id);
        //    db.warehouses.Remove(warehouse);
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
    }
}
