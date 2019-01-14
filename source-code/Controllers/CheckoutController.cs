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
            var order = new order();
            var user = new AllLoggedUserInfo();
            if (Session["ShoppingCart"] == null)
            {
                 return RedirectToAction("UpdateCart", "Home");
            }
            else
            {
             ViewBag.Cart = (ShoppingCart)Session["ShoppingCart"];
                if (Session["LoggedAccount"] == null)
                {
                    return View();
                }
                else
                {
                    user = (AllLoggedUserInfo)Session["LoggedAccount"];
                    var tblUser = db.users.Join(db.userdatas, us => us.Id, usdt => usdt.userid,
                        (us, usdt) => new AllModel { tblUser = us, tblUserData = usdt }).FirstOrDefault(x => x.tblUser.Id == user.user.Id);

                    if (tblUser != null)
                    {
                        order.fname = tblUser.tblUserData.firstname;
                        order.lname = tblUser.tblUserData.lasname;
                        order.email = tblUser.tblUser.email;
                        order.phone = tblUser.tblUserData.contact_phone;
                        order.d_fname = tblUser.tblUserData.delivery_name;
                        order.d_phone = tblUser.tblUserData.delivery_phone;
                        order.d_email = tblUser.tblUserData.delivery_email;
                        order.d_addr1 = tblUser.tblUserData.delivery_address1;
                        order.d_postcode = tblUser.tblUserData.delivery_postcode;
                        order.d_country = tblUser.tblUserData.delivery_contry;
                        order.b_fname = tblUser.tblUserData.billing_name;
                        order.b_phone = tblUser.tblUserData.billing_phone;
                        order.b_email = tblUser.tblUserData.billing_email;
                        order.b_addr1 = tblUser.tblUserData.billing_address1;
                        order.b_postcode = tblUser.tblUserData.billing_poscode;
                        order.b_country = tblUser.tblUserData.billing_country;

                    }

                }
            }
            ViewBag.ListCountry = db.countries.ToList();
             return View(order);
        }

        // POST: /Checkout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(order order)
        {
            if (ModelState.IsValid)
            {
                if (Session["ShoppingCart"] == null)
                {
                    return RedirectToAction("UpdateCart", "Home");
                }
                else
                {
                    try
                    {
                        order.d_companyname = string.IsNullOrEmpty(order.d_companyname)? order.companyname: order.d_companyname;
                        order.d_fname = string.IsNullOrEmpty(order.d_fname) ? order.fname: order.d_fname;
                        order.d_lname = string.IsNullOrEmpty(order.d_lname) ? order.lname: order.d_lname;
                        order.d_email = string.IsNullOrEmpty(order.d_email) ? order.email: order.d_email;
                        order.d_phone = string.IsNullOrEmpty(order.d_phone) ? order.phone: order.d_phone;
                        order.d_addr1 = string.IsNullOrEmpty(order.d_addr1) ? order.addr1: order.d_addr1;
                    
                        db.orders.Add(order);
                        db.SaveChanges();
                        ShoppingCart Cart = new ShoppingCart();
                        Cart = (ShoppingCart)Session["ShoppingCart"];

                        WebApplication1.Models.AllLoggedUserInfo userFullInfo = (WebApplication1.Models.AllLoggedUserInfo)Session["LoggedAccount"];
                        if (userFullInfo != null)
                        {
                            var user = db.users.Find(userFullInfo.user.Id);
                            user.paidorder = (user.paidorder ?? 0) + (decimal)Cart.CartTotal+ (decimal)(order.feeshipping ?? 0);
                            db.Entry(user).State = EntityState.Modified;
                        }



                        foreach (var item in Cart.cartItem)
                        {
                            orderdetail od = new orderdetail();
                            od.ocid = order.ocid;
                            od.ocdetailcode = item.Code;
                            od.ocdetailname = item.Name;
                            od.ocdetailprice = item.Price;
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
        public ActionResult Edit([Bind(Include = "ocid,fname,lname,companyname,tradingname,promotionName,abn,addr1,addr2,addr3,postcode,mobile,phone,fax,email,country,showsubtotal,clientID,submitDate,lastupdate,visible,tradeterms,ordertotal,ordertotalexc,status,currency,exchangerate,profileID,closedDate,d_attention,d_fname,d_lname,d_addr1,d_addr2,d_addr3,d_postcode,d_companyname,d_country,d_email,d_mobile,d_phone,d_fax,d_notes,payoption,tobeinvoiced,order_group,d_status,b_attention,b_fname,b_lname,b_companyname,b_abn,b_addr1,b_addr2,b_addr3,b_postcode,b_country,b_mobile,b_phone,b_fax,b_email,b_addrid,onhold,ocupdatedby,oclastupdated,notes")] order order)
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

        public ActionResult GetCity(int id=0)
        {
            if (id!=0)
            {
                var city = db.cities.Where(x => x.countryid == id).ToList();

                return Json(new { result = city }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFeeShipByCity(int id = 0)
        {
            if (id != 0)
            {
                var feeship = db.shippingfees.Where(x => x.cityid == id).ToList();

                return Json(new { result = feeship }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "" }, JsonRequestBehavior.AllowGet);

        }
    }
}
