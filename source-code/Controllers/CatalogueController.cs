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
    public class CatalogueController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        
        // GET: /Catalogue/
        public ActionResult Index()
        {
            return View(db.catalogues.ToList());
        }

        // GET: /Catalogue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catalogue catalogue = db.catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }
            return View(catalogue);
        }

        // GET: /Catalogue/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Catalogue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CatalogueCode,CatalgoueName,MetaGroupId")] catalogue catalogue)
        {
            if (ModelState.IsValid)
            {
                db.catalogues.Add(catalogue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogue);
        }

        // GET: /Catalogue/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catalogue catalogue = db.catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }
            return View(catalogue);
        }

        // POST: /Catalogue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatalogueCode,CatalogueName,MetaGroupId")] catalogue catalogue)
        {
            if (ModelState.IsValid)
            {

                var tem = db.catalogues.Where(m => m.Id == catalogue.Id).FirstOrDefault();
                tem.CatalogueCode = catalogue.CatalogueCode;
                tem.CatalogueName = catalogue.CatalogueName;
                tem.MetaGroupId = catalogue.MetaGroupId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalogue);
        }

        // GET: /Catalogue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catalogue catalogue = db.catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }
            return View(catalogue);
        }

        // POST: /Catalogue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            catalogue catalogue = db.catalogues.Find(id);
            db.catalogues.Remove(catalogue);
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
