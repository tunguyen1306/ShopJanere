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

namespace WebApplication1.Controllers
{
    public class CountryController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /country/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listcountry = db.countries.ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listcountry.Count;
            listcountry= listcountry.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listcountry.Count;
          
            return PartialView(listcountry);
        }
        // GET: /country/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: /country/Create
        public ActionResult Create()
        {
            return View(new country());
        }

        // POST: /country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create( country country, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != null)
                        {

                            string path = "";
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(files[i].FileName));
                            var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                            files[i].SaveAs(path);
                            country.urlimage = "/Content/ProductImage/" + files[i].FileName;


                        }
                    }
                }
                db.countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: /country/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: /country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit( country country, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != null)
                        {

                            string path = "";
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(files[i].FileName));
                            var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                            files[i].SaveAs(path);
                            country.urlimage = "/Content/ProductImage/" + files[i].FileName;


                        }
                    }
                }
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: /country/Delete/5
        public ActionResult Delete(int? id)
        {
            country country = db.countries.Find(id);
            db.countries.Remove(country);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /country/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    country country = db.countries.Find(id);
        //    db.countries.Remove(country);
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
