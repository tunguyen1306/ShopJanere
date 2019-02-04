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
    public class cityController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /city/
        public ActionResult Index()
        {
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            list.Insert(0, new country { id = 0, name = "Select Country" });
            ViewBag.ListTypecity = list;
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listcity = db.cities.Join(db.countries, st => st.countryid, stt => stt.code, (st, stt) => new AllModel { tblCity = st, tblCountry = stt }).ToList();
            
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listcity.Count;
            listcity = listcity.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listcity.Count;

            return PartialView(listcity);
        }
        // GET: /city/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var city = db.cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: /city/Create
        public ActionResult Create()
        {
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;

            return View(new city());
        }

        // POST: /city/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(city city)
        {
            if (ModelState.IsValid)
            {
             
                db.cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(city);
        }

        // GET: /city/Edit/5
        public ActionResult Edit(int? id)
        {
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            city city = db.cities.Find(id);
            if (city == null)
            {
                
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: /city/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(city city, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: /city/Delete/5
        public ActionResult Delete(int? id)
        {
            city city = db.cities.Find(id);
            db.cities.Remove(city);
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
