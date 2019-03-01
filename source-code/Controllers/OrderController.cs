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
        public ActionResult Index(string tab="all")
        {
            var listOrderStatus = db.orderstatus.ToList();
            listOrderStatus.Insert(0,new orderstatu{id = 0,name = "Select Action"});
            ViewBag.OrderStatus = listOrderStatus;
            ViewBag.Tab = tab;
            ViewBag.idMenu = tab;
            return View();
        }
        public ActionResult IndexAjax(string tab="all",string datepicker = null, string search_order = null, int start = 0, int view = 10)
        {
            ViewBag.idMenu = tab;
            var listOrder = db.orders;
            var listStatus = db.orderstatus.Select(x => x.id.ToString()).ToList();

            var listAll = (from pro in listOrder
                           select new { tblOrder = pro });
            if (tab != "all")
            {
                
                if (tab=="1")
                {
                    listAll = listAll.Where(x => x.tblOrder.status == tab || x.tblOrder.status==null);
                }
                else
                {
                    listAll = listAll.Where(x => x.tblOrder.status == tab);
                }
            }
            else
            {
                listAll = listAll.Where(x => listStatus.Contains(x.tblOrder.status));
            }
           
            
            

            if (!string.IsNullOrEmpty(datepicker))
            {
                var date = DateTime.ParseExact(datepicker, "dd/MM/yyyy", null);
                listAll = listAll.Where(x => x.tblOrder.submitDate <= date);
            }
            if (search_order != null)
            {
                listAll = listAll.Where(x => x.tblOrder.ocid.ToString().Contains(search_order) || x.tblOrder.d_addr1.ToString().Contains(search_order) || x.tblOrder.b_fname.ToString().Contains(search_order) || x.tblOrder.b_lname.ToString().Contains(search_order));
            }
            var _count = listAll.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = _count;
            ViewBag.ViewOf = _count;
            var db_data = listAll.OrderBy(t => t.tblOrder.ocid).Skip(start).Take(view).OrderByDescending(x => x.tblOrder.status).ToList();
            ViewBag.Tab = tab;

            var datas = db_data.Select(t => new AllModel { tblOrder = t.tblOrder }).ToList();
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
            var list = db.items.Join(db.artgrps, it => it.GROUPNO, grp => grp.GROUPNO, (it, grp) => new AllModel { tblitem = it, tblGroup = grp }).Where(x => x.tblitem.ARTNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTNAME, des = x.tblitem.INFO, grp = x.tblGroup.GROUPNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            var listOrderStatus = db.orderstatus.ToList();
            listOrderStatus.Insert(0, new orderstatu { id = 0, name = "Select Action" });
            ViewBag.OrderStatus = listOrderStatus;
            var proId = db.orders.OrderByDescending(x => x.ocid).FirstOrDefault();
            var idoder = 0;
            if (proId != null)
            {
                idoder = (int)proId.ocid + 1;
            }
            else
            {
                idoder = 1;
            }
            ViewBag.IdOrder = idoder;
            ViewBag.DateOrder = DateTime.Now.Date.ToShortDateString();
            ViewBag.HourOrder = DateTime.Now.Hour;
            ViewBag.MuniteOrder = DateTime.Now.Minute;

            return View(new order());
        }

        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(order order, string[] totalProduct)
        {


            try
            {

              
                order.submitDate = DateTime.Now;
                db.orders.Add(order);
                db.SaveChanges();
                if (totalProduct != null)
                {
                    var listItem = totalProduct.ToList().Distinct().ToList();
                    foreach (var item in listItem)
                    {
                        var itemId = int.Parse(item.Split('_')[0]);
                        var tblItem = db.items.FirstOrDefault(x => x.ARTNO == itemId);
                        var tblOrderDetail = db.orderdetails.FirstOrDefault(x => x.itemId == itemId && x.ocid == order.ocid);
                        if (tblOrderDetail == null)
                        {
                            var tblOrder = new orderdetail
                            {
                                ocid = order.ocid,
                                itemId = itemId,
                                ocdetailqty = int.Parse(item.Split('_')[1]),
                                ocdetailname = tblItem != null ? tblItem.ARTNAME : ""

                            };
                            db.orderdetails.Add(tblOrder);
                        }
                        else
                        {
                            tblOrderDetail.ocdetailqty = int.Parse(item.Split('_')[1]);
                            db.Entry(tblOrderDetail).State = EntityState.Modified;
                        }

                    }
                    db.SaveChanges();
                }


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
            var listOrderStatus = db.orderstatus.ToList();
            listOrderStatus.Insert(0, new orderstatu { id = 0, name = "Select Action" });
            ViewBag.OrderStatus = listOrderStatus;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var listDetail =
                db.orderdetails.Join(db.orders, x => x.ocid, x1 => x1.ocid, (x, x1) => new AllModel { tblOrder = x1, tblOrderDetail = x }).Where(x => x.tblOrderDetail.ocid == id).ToList();
            ViewBag.ListOderDetail = listDetail;
            return View(order);
        }

        // POST: /Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(order order, string[] totalProduct)
        {

            //var tem = db.orders.Find(order.ocid);

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            if (totalProduct != null)
            {
                var listItem = totalProduct.ToList().Distinct().ToList();
                foreach (var item in listItem)
                {
                    var itemId = int.Parse(item.Split('_')[0]);
                    var tblItem = db.items.FirstOrDefault(x => x.ARTNO == itemId);
                    var tblOrderDetail = db.orderdetails.FirstOrDefault(x => x.itemId == itemId && x.ocid == order.ocid);
                    if (tblOrderDetail == null)
                    {
                        var tblOrder = new orderdetail
                        {
                            ocid = order.ocid,
                            itemId = itemId,
                            ocdetailqty = int.Parse(item.Split('_')[1]),
                            ocdetailname = tblItem != null ? tblItem.ARTNAME : ""

                        };
                        db.orderdetails.Add(tblOrder);
                    }
                    else
                    {
                        tblOrderDetail.ocdetailqty = int.Parse(item.Split('_')[1]);
                        db.Entry(tblOrderDetail).State = EntityState.Modified;
                    }

                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET: /Order/Delete/5
        public ActionResult Delete(int? id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult DeleteOrderDetail(int? id)
        {
            orderdetail order = db.orderdetails.Find(id);
            if (order != null)
            {

                db.orderdetails.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Edit", "Order", new { id = order.ocid });
            }
            return RedirectToAction("Edit", "Order", new { id = order.ocid });


        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult UpdateStatus(string status, string[] ids)
        {
            var _ids = ids.Select(t => int.Parse(t)).ToList();
            bool _status = false;
            var msg = "Update Success";
            try
            {

                var _items = db.orders.ToList().Where(x => _ids.Contains((int)x.ocid));
                foreach (var item in _items.ToList())
                {
                    var tbl = db.orders.Find(item.ocid);
                    if (tbl != null)
                    {
                        tbl.status = status;
                        db.Entry(tbl).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                _status = true;
            }
            catch (Exception ex)
            {

                msg = "Update Failed";
            }
            return Json(new { status = _status, mgs = msg });
        }

        public ActionResult SettingIndex()
        {
       
            return View();
        }
        public ActionResult SettingIndexAjax( int start = 0, int view = 10)
        {

            var listOrder = db.ordersettings.ToList();



            var datas = listOrder.Select(t => new AllModel { tblOrderSetting = t }).ToList();
            var count = datas.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = count;
            ViewBag.ViewOf = count;
            var dbData = datas.OrderBy(t => t.tblOrderSetting.id).Skip(start).Take(view).ToList();


           
            return PartialView(dbData);


        }

        // GET: /setting/Create
        public ActionResult CreateOrderSetting()
        {
            
            return View(new ordersetting());
        }

      
        [HttpPost]

        public ActionResult CreateOrderSetting(ordersetting ordersetting)
        {
            if (ModelState.IsValid)
            {
               
                db.ordersettings.Add(ordersetting);
                db.SaveChanges();
                return RedirectToAction("SettingIndex");
            }

            return View(ordersetting);
        }

        // GET: /setting/Edit/5
        public ActionResult EditOrderSetting(int? id)
        {
        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordersetting setting = db.ordersettings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

       
        [HttpPost]

        public ActionResult EditOrderSetting(ordersetting setting)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SettingIndex");
            }
            return View(setting);
        }


        public ActionResult DeleteOrderSetting(int? id)
        {
            setting setting = db.settings.Find(id);
            db.settings.Remove(setting);
            db.SaveChanges();
            return RedirectToAction("SettingIndex");
        }

       
        public ActionResult UpdateStatusSetting(int? id)
        {
            var list = db.ordersettings.ToList();
            foreach (var item in list)
            {
                var tbl = db.ordersettings.Find(item.id);
                tbl.status = 0;
                db.Entry(tbl).State=EntityState.Modified;
               
            }
            var tbl1 = db.ordersettings.Find(id);
            tbl1.status = 1;
            db.Entry(tbl1).State = EntityState.Modified;
            db.SaveChanges();

            MvcApplication.SyncMoneySymbol();

            return RedirectToAction("SettingIndex");
        }



    }
}

