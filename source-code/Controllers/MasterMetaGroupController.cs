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
    public class MasterMetaGroupController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /MasterMetaGroup/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listMetagroup = (from cat in db.metagrups
                                 //join catl in db.metagrups on cat.MetaGroupId equals catl.METAGROUPNO, tblMetaGroup = catl
                                 select new AllModel { tblMetaGroup = cat }).ToList();



            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listMetagroup.Count;
            listMetagroup = listMetagroup.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listMetagroup.Count;

            return PartialView(listMetagroup);
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
            return View(new metagrup());
        }

        // POST: /MasterMetaGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create( metagrup metagrup)
        {
            metagrup.IsActive = true;
            metagrup.CREATED = DateTime.Now;
                db.metagrups.Add(metagrup);
                db.SaveChanges();
                return RedirectToAction("Index");

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
        public ActionResult Edit(metagrup metagrup)
        {
            if (ModelState.IsValid)
            {
                metagrup.LASTCHANGE = DateTime.Now;
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
            db.metagrups.Remove(metagrup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
