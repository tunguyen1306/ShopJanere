using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /Category/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listStore = (from cat in db.categories
                join catl in db.catalogues on cat.CatalogueCode equals catl.Id
                select new AllModel { tblCategory=cat,tblCatalog= catl}).ToList();

          
            
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listStore.Count;
            listStore = listStore.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listStore.Count;

            return PartialView(listStore);
        }
        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCatalog = db.catalogues.ToList();
            return View(new category{CREATED = DateTime.Now,LASTCHANGE = DateTime.Now});
        }

        // POST: /Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public ActionResult Create( category category)
        {
            if (ModelState.IsValid)
            {
                category.CREATED = DateTime.Now;
                category.LASTCHANGE = DateTime.Now;
                db.categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: /Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListCatalog = db.catalogues.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Edit(category category)
        {
            if (ModelState.IsValid)
            {
                category.LASTCHANGE = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: /Category/Delete/5
        public ActionResult Delete(int? id)
        {
            category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            category category = db.categories.Find(id);
            db.categories.Remove(category);
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
