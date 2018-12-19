using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CheckoutController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /Checkout/
        public ActionResult Index()
        {
            return View(db.orders.ToList());
        }

        // GET: /Checkout/Details/5
        public ActionResult Details(long? id)
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

        // GET: /Checkout/Create
        public ActionResult Create()
        {
            if (Session["ShoppingCart"] == null)
                return RedirectToAction("UpdateCart", "Home");
            return View();
        }

        // POST: /Checkout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ocid,fname,lname,companyname,tradingname,promotionName,abn,addr1,addr2,addr3,postcode,mobile,phone,fax,email,country,showsubtotal,clientID,submitDate,lastupdate,visible,tradeterms,ordertotal,ordertotalexc,status,currency,exchangerate,profileID,closedDate,d_attention,d_fname,d_lname,d_addr1,d_addr2,d_addr3,d_postcode,d_companyname,d_country,d_email,d_mobile,d_phone,d_fax,d_notes,payoption,tobeinvoiced,order_group,d_status,b_attention,b_fname,b_lname,b_companyname,b_abn,b_addr1,b_addr2,b_addr3,b_postcode,b_country,b_mobile,b_phone,b_fax,b_email,b_addrid,onhold,ocupdatedby,oclastupdated,notes")] order order)
        {
            if (ModelState.IsValid)
            {
                if (Session["ShoppingCart"] == null)
                {
                    return RedirectToAction("UpdateCart", "Home");
                }
                else
                {
                    try { 
                    db.orders.Add(order);
                    db.SaveChanges();
                    ShoppingCart Cart = new ShoppingCart();
                    Cart = (ShoppingCart)Session["ShoppingCart"];

                    WebApplication1.Models.AllLoggedUserInfo userFullInfo = (WebApplication1.Models.AllLoggedUserInfo)Session["LoggedAccount"];
                    if (userFullInfo!=null)
                    {
                            var user = db.users.Find(userFullInfo.user.Id);
                            user.paidorder = (user.paidorder.HasValue? user.paidorder.Value:0)+(decimal)Cart.CartTotal;
                            db.Entry(user).State = EntityState.Modified;
                    }



                        foreach (var item in Cart.cartItem)
                    {
                        orderdetail od = new orderdetail();
                        od.ocid = order.ocid;
                        od.ocdetailcode = item.Code;
                        od.ocdetailname = item.Name;                        
                        od.ocdetailprice = item.Price==null?0:item.Price;
                        od.ocdetailqty = item.Qty;
                        od.ocdetailgst = item.Tax;
                        db.orderdetails.Add(od);
                    }

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
                }
                return RedirectToAction("Thankyou");
               // return RedirectToAction("Index");
            }

            return View(order);
        }
        public ActionResult Thankyou()
        {
            return View();
        }
        // GET: /Checkout/Edit/5
        public ActionResult Edit(long? id)
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

        // POST: /Checkout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ocid,fname,lname,companyname,tradingname,promotionName,abn,addr1,addr2,addr3,postcode,mobile,phone,fax,email,country,showsubtotal,clientID,submitDate,lastupdate,visible,tradeterms,ordertotal,ordertotalexc,status,currency,exchangerate,profileID,closedDate,d_attention,d_fname,d_lname,d_addr1,d_addr2,d_addr3,d_postcode,d_companyname,d_country,d_email,d_mobile,d_phone,d_fax,d_notes,payoption,tobeinvoiced,order_group,d_status,b_attention,b_fname,b_lname,b_companyname,b_abn,b_addr1,b_addr2,b_addr3,b_postcode,b_country,b_mobile,b_phone,b_fax,b_email,b_addrid,onhold,ocupdatedby,oclastupdated,notes")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: /Checkout/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: /Checkout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
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
    }
}
