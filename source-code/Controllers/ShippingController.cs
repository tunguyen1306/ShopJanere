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
    public class ShippingController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /shippingfee/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listshippingfee = db.shippingfees.Select(x => new AllModel { tblShipping = x }).ToList();
            
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listshippingfee.Count;
            listshippingfee = listshippingfee.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listshippingfee.Count;

            return PartialView(listshippingfee);
        }
        // GET: /shippingfee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var shippingfee = db.shippingfees.Find(id);
            if (shippingfee == null)
            {
                return HttpNotFound();
            }
            return View(shippingfee);
        }

        // GET: /shippingfee/Create
        public ActionResult Create()
        {
            var list = db.cities.Where(x => x.status == 1).ToList();
            ViewBag.ListCiTy = list;

            return View(new shippingfee());
        }

        // POST: /shippingfee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(shippingfee shippingfee)
        {
            if (ModelState.IsValid)
            {
             
                db.shippingfees.Add(shippingfee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shippingfee);
        }

        // GET: /shippingfee/Edit/5
        public ActionResult Edit(int? id)
        {

            var list = db.cities.Where(x => x.status == 1).ToList();
            ViewBag.ListCiTy = list;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shippingfee shippingfee = db.shippingfees.Find(id);
            if (shippingfee == null)
            {
                
                return HttpNotFound();
            }
            return View(shippingfee);
        }

        // POST: /shippingfee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(shippingfee shippingfee, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(shippingfee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shippingfee);
        }

        // GET: /shippingfee/Delete/5
        public ActionResult Delete(int? id)
        {
            shippingfee shippingfee = db.shippingfees.Find(id);
            db.shippingfees.Remove(shippingfee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      

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
