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
    public class MasterMetaGroupController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /MasterMetaGroup/
        public ActionResult Index()
        {
            return View(db.metagrups.ToList());
        }

        // GET: /MasterMetaGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metagrup metagrup = db.metagrups.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }

        // GET: /MasterMetaGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MasterMetaGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="METAGROUPNO,METAGROUPCODE,METAGROUPNAME,PARENTNO,LASTCHANGE,CREATED,IsActive")] metagrup metagrup)
        {
            if (ModelState.IsValid)
            {
                db.metagrups.Add(metagrup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(metagrup);
        }

        // GET: /MasterMetaGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metagrup metagrup = db.metagrups.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }

        // POST: /MasterMetaGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="METAGROUPNO,METAGROUPCODE,METAGROUPNAME,PARENTNO,LASTCHANGE,CREATED,IsActive")] metagrup metagrup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metagrup).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(metagrup);
        }

        // GET: /MasterMetaGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metagrup metagrup = db.metagrups.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }

        // POST: /MasterMetaGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            metagrup metagrup = db.metagrups.Find(id);
            db.metagrups.Remove(metagrup);
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
