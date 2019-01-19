
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using PayPal;
using PayPal.Api;
using WebApplication1;
using WebApplication1.Helper;
using WebApplication1.Models;
using UrlHelper = WebApplication1.Helper.UrlHelper;
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
                ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
                if (cart.cartItem.Count==0)
                {
                    return RedirectToAction("UpdateCart", "Home");
                }

                ViewBag.Cart = cart;
                if (Session["LoggedAccount"] == null)
                {
                    return View();
                }
                else
                {

                    if (string.IsNullOrWhiteSpace( cart.paid_key))
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
                    else
                    {
                        order =  db.orders.Where(t => t.paid_key == cart.paid_key).FirstOrDefault();
                        if (order==null)
                        {
                            order = new order();
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
                    

                }
            }
            ViewBag.ListCountry = db.countries.ToList();
             return View(order);
        }

        // POST: /Checkout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(order order,int? checkBilldingShipping)
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
                        var oldPriceOrder = 0.0;
                        if (order.ocid>0)
                        {
                            var _order = db.orders.Where(t=>t.ocid==order.ocid).FirstOrDefault();
                            oldPriceOrder = _order.paid_amount.HasValue ? _order.paid_amount.Value : 0;
                            db.Entry(_order).State = EntityState.Detached;
                            db.SaveChanges();
                        }
                        order.d_companyname = string.IsNullOrEmpty(order.d_companyname)? order.companyname: order.d_companyname;
                        order.d_fname = string.IsNullOrEmpty(order.d_fname) ? order.fname: order.d_fname;
                        order.d_lname = string.IsNullOrEmpty(order.d_lname) ? order.lname: order.d_lname;
                        order.d_email = string.IsNullOrEmpty(order.d_email) ? order.email: order.d_email;
                        order.d_phone = string.IsNullOrEmpty(order.d_phone) ? order.phone: order.d_phone;
                        order.d_addr1 = string.IsNullOrEmpty(order.d_addr1) ? order.addr1: order.d_addr1;
                        if (checkBilldingShipping.HasValue && checkBilldingShipping.Value==1)
                        {
                            order.b_companyname = string.IsNullOrEmpty(order.d_companyname) ? order.companyname : order.d_companyname;
                            order.b_fname = string.IsNullOrEmpty(order.d_fname) ? order.fname : order.d_fname;
                            order.b_lname = string.IsNullOrEmpty(order.d_lname) ? order.lname : order.d_lname;
                            order.b_email = string.IsNullOrEmpty(order.d_email) ? order.email : order.d_email;
                            order.b_phone = string.IsNullOrEmpty(order.d_phone) ? order.phone : order.d_phone;
                            order.b_addr1 = string.IsNullOrEmpty(order.d_addr1) ? order.addr1 : order.d_addr1;                           

                        }
                        order.status = "2";
                        if (order.payoption== "PayPal")
                        {
                            order.paid_status = 2;
                        }
                        if (order.payoption == "COB")
                        {
                            order.paid_status = 1;
                           
                        }
                        ShoppingCart Cart = new ShoppingCart();
                        Cart = (ShoppingCart)Session["ShoppingCart"];
                        order.paid_amount = Cart.CartTotal + Cart.taxTotal;
                        String randomKey = Guid.NewGuid().ToString();
                        if (order.ocid==0)
                        {
                           
                            order.paid_key = randomKey;
                            db.orders.Add(order);
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();
                        }                     
                      
                        Cart.paid_key = order.paid_key;
                        WebApplication1.Models.AllLoggedUserInfo userFullInfo = (WebApplication1.Models.AllLoggedUserInfo)Session["LoggedAccount"];
                        if (userFullInfo != null)
                        {
                            var user = db.users.Find(userFullInfo.user.Id);
                            user.paidorder= (user.paidorder ?? 0) + (decimal)Cart.CartTotal+ (decimal)(order.feeshipping ?? 0)- (decimal)oldPriceOrder;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        double cartAmount = 0;
                        string currencyName = "USD";
                        ordersetting ordersetting = db.ordersettings.Where(t => t.status == 1).FirstOrDefault();
                        if (ordersetting != null)
                        {
                            currencyName = ordersetting.name;
                        }

                        var itemList = new ItemList();
                        var items = new List<Item>();
                      
                        String paypalURL = "";
                        var paypayconfig = new PayPalConfiguration();
                        var apiContext = paypayconfig.GetAPIContext();

                        var _orderdetails =  db.orderdetails.Where(t => t.ocid == order.ocid).ToList();
                        var listItems = _orderdetails.Select(t => t.ocdetailcode);
                        var _items= db.items.Where(t => listItems.Contains(t.ARTCODE)).ToList();
                        foreach (var item in _orderdetails)
                        {
                          
                            if (item.stockId.HasValue && item.stockId.Value>0)
                            {
                               var _item=  _items.FirstOrDefault(t => t.ARTCODE == item.ocdetailcode);
                                if (_item!=null)
                                { 
                                   var _stock= db.stocks.Where(t => t.ARTNO == item.itemId && t.STOCKNO == item.stockId).FirstOrDefault();
                                    _stock.VOLUME += item.ocdetailqty.Value;
                                    db.Entry(_stock).State = EntityState.Modified;
                                }
                               
                            }
                            
                        }
                        db.orderdetails.RemoveRange(_orderdetails);
                        var _listItems = Cart.cartItem.Select(t => t.Code);
                        var __items = db.items.Where(t => _listItems.Contains(t.ARTCODE)).ToList();
                        foreach (var item in Cart.cartItem)
                        {
                            orderdetail od = new orderdetail();
                            od.ocid = order.ocid;
                            od.ocdetailcode = item.Code;
                            od.ocdetailname = item.Name;
                            od.ocdetailprice = item.Price;
                            od.ocdetailqty = item.Qty;
                            od.ocdetailgst = item.Tax/ item.Qty;
                            od.stockId = item.StockId;
                            if (item.StockId>0)
                            {
                                var _item = __items.FirstOrDefault(t => t.ARTCODE == od.ocdetailcode);
                                if (_item != null)
                                {
                                    var _stock = db.stocks.Where(t => t.ARTNO == _item.ARTNO && t.STOCKNO == item.StockId).FirstOrDefault();
                                    _stock.VOLUME -= item.Qty;
                                    db.Entry(_stock).State = EntityState.Modified;
                                    od.itemId = _item.ARTNO;
                                }

                            }
                            db.orderdetails.Add(od);

                            var Item = new Item();
                            Item.name = item.Code+" - "+ item.Name;
                            Item.currency = currencyName;
                            
                            Item.price = (item.Price + (item.Tax / item.Qty)) + "";
                            Item.quantity = item.Qty + "";
                            items.Add(Item);

                        }
                        if (order.feeshipping.HasValue && order.feeshipping.Value>0)
                        {
                            var Item = new Item();
                            Item.name = "Fee Shipping";
                            Item.currency = currencyName;
                            Item.price = order.feeshipping + "";
                            Item.quantity =  "1";
                            items.Add(Item);
                        }
                        if (Cart.promotion != null)
                        {
                            if (Cart.promotion.TYPENO ==0)
                            {
                                var Item = new Item();
                                Item.name = "Discount Promotion";
                                Item.currency = currencyName;
                                Item.price ="-"+ Cart.PromotionTotal + "";
                                Item.quantity = "1";
                                items.Add(Item);
                            }
                        }
                        cartAmount = Cart.CartTotal+Cart.taxTotal+ order.feeshipping??0;
                        cartAmount = Math.Round(cartAmount, 2);
                        itemList.items = items;
                        db.SaveChanges();
                        if (order.payoption=="PayPal")
                        {
                            var payer = new Payer() { payment_method = "paypal" };
                            var redirUrls = new RedirectUrls()
                            {
                                cancel_url = UrlHelper.Root + "Checkout/PayPalCancel?" + UrlHelper.ToQueryString(new { paid_key = order.paid_key }),
                                return_url = UrlHelper.Root + "Checkout/PayPalSuccess?" + UrlHelper.ToQueryString(new { paid_key = order.paid_key })
                            };
                            var paypalAmount = new Amount() { currency = currencyName, total = cartAmount.ToString() };

                            var transactionList = new List<PayPal.Api.Transaction>();
                            PayPal.Api.Transaction transaction = new PayPal.Api.Transaction();
                            transaction.amount = paypalAmount;
                            transaction.item_list = itemList;
                            transactionList.Add(transaction);


                            var payment = new Payment()
                            {
                                intent = "Sale",
                                payer = payer,
                                transactions = transactionList,
                                redirect_urls = redirUrls
                            };

                            try
                            {
                                var createdPayment = payment.Create(apiContext);
                                var links = createdPayment.links.GetEnumerator();
                                while (links.MoveNext())
                                {
                                    var link = links.Current;
                                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                                    {
                                        paypalURL = link.href;
                                    }
                                }
                                return Redirect(paypalURL);
                            }
                            catch (PaymentsException ex)
                            {
                                paypalURL = "ERROR: " + ex.Response;
                            }
                        }
                        else
                        {
                            
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
                }
                return RedirectToAction("Thankyou");
                // return RedirectToAction("Index");
            }

            return View(order);
        }
        public bool SendTemplateEmail(string recepientEmail, string username, string key, string Subject, int type)
        {
            bool t = false;
            //Type =1 MailOrder
            //Type =2 ForgetPass
            string body = string.Empty;
            var activelink = "";
        
            if (type == 1)
            {
                var check = db.users.ToList().FirstOrDefault(x => x.email == recepientEmail.ToLower());
                if (check != null)
                {
                   

                    body = ViewRenderer.RenderPartialView("~/Views/Shared/Partial/_ResetPassTemplateMail.cshtml");
                    body = body.Replace("##name##", username);
                  
                    
                }
            }

t = Models.Helper.SendEmail("donotreply@example.com", recepientEmail, Subject, body);


            return t;
        }
        public ActionResult PayPalSuccess(string paid_key, string paymentId, string token, string PayerID)
        {
            var order=  db.orders.Where(t => t.paid_key == paid_key).FirstOrDefault();
            if (order==null)
            {
                return HttpNotFound();
            }
            order.status = "5";
            order.paid_key = order.paid_key + "_" + token + "_" + paymentId + "_" + PayerID;
            order.paid_status = 3;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
         
            return RedirectToAction("Thankyou");
        }
        public ActionResult PayPalCancel(string paid_key, string token)
        {
            if (String.IsNullOrWhiteSpace(paid_key))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Where(t => t.paid_key == paid_key).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OderDetails = db.orderdetails.Where(t => t.ocid == order.ocid).ToList();
            
            return RedirectToAction("Create");
        }
        public ActionResult Thankyou()
        {
              Session["ShoppingCart"] = new ShoppingCart();
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
            ViewBag.OderDetails = db.orderdetails.Where(t => t.ocid == order.ocid).ToList();
            ViewBag.ListCountry = db.countries.ToList();
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
