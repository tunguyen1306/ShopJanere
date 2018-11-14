using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using LinqToExcel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        public ActionResult Index(String tab = "all")
        {
           
            ViewBag.Tab = tab;
            return View();
        }
        public ActionResult IndexAjax(string tab = "all", int start = 0, int view = 10)
        {

            var listOrder = db.orders;
            
          
            var listAll = (from pro in listOrder
                           select new { tblOrder = pro });


            var _count = listAll.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = _count;
            ViewBag.ViewOf = _count;
            var db_data = listAll.OrderBy(t => t.tblOrder.ocid).Skip(start).Take(view).ToList();
           
           
            var datas = db_data.Select(t => new AllModel {  tblOrder = t.tblOrder }).ToList();
            return PartialView(datas);


        }

     

        // GET: /Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        public ActionResult _AjaxAutoComplete(string query)
        {
            var list = db.items.Join(db.artgrps,it=>it.GROUPNO,grp=>grp.GROUPNO,(it,grp)=>new AllModel {tblitem =it,tblGroup = grp}).Where(x => x.tblitem.ARTNAME.ToLower().Contains(query.ToLower())).ToList().Select(x=>new  { value = x.tblitem.ARTNO,label= x.tblitem.ARTNAME,des=x.tblitem.INFO,grp=x.tblGroup.GROUPNAME}).ToList();
            return Json(new { status = true,data = list},JsonRequestBehavior.AllowGet);
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            var proId = db.orders.OrderByDescending(x => x.ocid).FirstOrDefault();
            var idoder = 0;
            if (proId!=null)
            {
                idoder = (int)proId.ocid + 1; 
            }
            else
            {
                idoder = 1;
            }
            ViewBag.IdOrder = idoder;
            return View(new order());
        }
        
        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(order order, HttpPostedFileBase[] inputfile)
        {
           
      
            try
            {

                var proId = db.orders.OrderByDescending(x => x.ocid).FirstOrDefault();
              
                order.ocid = proId.ocid + 1;
                db.orders.Add(order);
                db.SaveChanges();
      
               
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: /Order/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(order order, HttpPostedFileBase[] inputfile)
        {

            var tem = db.orders.Find(order.ocid);
         
            db.Entry(tem).State = EntityState.Modified;
            db.SaveChanges();
          
            return RedirectToAction("Index");
        }
        // GET: /Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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

     
   
     
        public ActionResult DeleteImg(int? id)
        {
            
            var order = db.artlinks.Find(id);
            if (order!=null)
            {
                db.artlinks.Remove(order);
                db.SaveChanges();
            }
            
            return Json("");
        }
      
    }
}

