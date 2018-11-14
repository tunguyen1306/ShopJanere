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
    public class MenuController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /menu/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            //var listmenu = db.menus.Join(db.menus,mn1=>mn1.parentId,mn2=>mn2.id,(mn1,mn2)=>new AllModel{tblMenu = mn1,tblParentMenu = mn2}).SelectMany(x => x.tblParentMenu.name.DefaultIfEmpty(),(x, y) => new { tblMenu = x.tblMenu, tblParentMenu = y }).ToList();


          var  listmenu = db.menus.GroupJoin(
                       db.menus,
                      foo => foo.parentId,
                      bar => bar.id,
                      (x, y) => new { Foo = x, Bars = y })
                .SelectMany(
                      x => x.Bars.DefaultIfEmpty(),
                      (x, y) => new AllModel { tblMenu = x.Foo, tblParentMenu = y }).ToList();



            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listmenu.Count;
            listmenu= listmenu.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listmenu.Count;
          
            return PartialView(listmenu);
        }
        // GET: /menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            menu menu = db.menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: /menu/Create
        public ActionResult Create()
        {
            var listParent = db.menus.Where(x => x.isparent == 1 && x.parentId == 0).ToList();
            ViewBag.ListParentMenu = listParent;
            return View(new menu());
        }

        // POST: /menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create( menu menu)
        {
            if (ModelState.IsValid)
            {
                db.menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: /menu/Edit/5
        public ActionResult Edit(int? id)
        {
            var listParent = db.menus.Where(x => x.isparent == 1 && x.parentId == 0).ToList();
            ViewBag.ListParentMenu = listParent;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            menu menu = db.menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: /menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit( menu menu)
        {
            if (ModelState.IsValid)
            {
             
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: /menu/Delete/5
        public ActionResult Delete(int? id)
        {
            menu menu = db.menus.Find(id);
            db.menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /menu/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    menu menu = db.menus.Find(id);
        //    db.menus.Remove(menu);
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
