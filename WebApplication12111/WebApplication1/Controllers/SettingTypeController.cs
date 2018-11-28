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
    public class settingtypeController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /settingtype/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listsettingtype = db.settingtypes.ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listsettingtype.Count;
            listsettingtype= listsettingtype.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listsettingtype.Count;
          
            return PartialView(listsettingtype);
        }
        // GET: /settingtype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            settingtype settingtype = db.settingtypes.Find(id);
            if (settingtype == null)
            {
                return HttpNotFound();
            }
            return View(settingtype);
        }

        // GET: /settingtype/Create
        public ActionResult Create()
        {
            return View(new settingtype());
        }

        // POST: /settingtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create( settingtype settingtype)
        {
            if (ModelState.IsValid)
            {
                db.settingtypes.Add(settingtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(settingtype);
        }

        // GET: /settingtype/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            settingtype settingtype = db.settingtypes.Find(id);
            if (settingtype == null)
            {
                return HttpNotFound();
            }
            return View(settingtype);
        }

        // POST: /settingtype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit( settingtype settingtype)
        {
            if (ModelState.IsValid)
            {
             
                db.Entry(settingtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(settingtype);
        }

        // GET: /settingtype/Delete/5
        public ActionResult Delete(int? id)
        {
            settingtype settingtype = db.settingtypes.Find(id);
            db.settingtypes.Remove(settingtype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /settingtype/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    settingtype settingtype = db.settingtypes.Find(id);
        //    db.settingtypes.Remove(settingtype);
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
