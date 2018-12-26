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
    public class AccountLoginController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /AccountLogin/
        public ActionResult Index()
        {

            return View(db.users);
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listuser = db.users.ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listuser.Count;
            listuser = listuser.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listuser.Count;

            return PartialView(listuser);
        }

        // GET: /AccountLogin/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //accountlogin accountlogin = db.accountlogins.Find(id);
            //if (accountlogin == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: /AccountLogin/Create
        public ActionResult Create()
        {
            user user = new user();
            return View(user);
        }
        public ActionResult CreateUser()
        {
            user user = new user();
            return View(user);
        }
        [HttpPost]

        public ActionResult CreateUser(user user)
        {
            if (ModelState.IsValid)
            {

                user.status = "active";
                user.createdate = DateTime.Now;
                user.updatedate = DateTime.Now;
                db.users.Add(user);
                db.SaveChanges();
                user_role ur = new user_role();
                ur.userid = user.Id;
                ur.roleid = 7;
                ur.status = "active";
                db.user_role.Add(ur);

                userdata ud = new userdata();
                ud.userid = user.Id;
                db.userdatas.Add(ud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult EditUser(int? id)
        {
            user user = db.users.Find(id);
            var role = db.user_role.FirstOrDefault(t => t.status == "active" && t.userid == id);
            ViewBag.Role = role?.roleid ?? -1;
            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(user model)
        {
            var user = db.users.Find(model.Id);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: /AccountLogin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create(user user)
        {
            if (ModelState.IsValid)
            {
                
                user.status = "active";
                user.createdate = DateTime.Now;
                user.updatedate = DateTime.Now;
                db.users.Add(user);
                db.SaveChanges();
                user_role ur = new user_role();
                ur.userid = user.Id;
                ur.roleid = 7;
                ur.status = "active";
                db.user_role.Add(ur);
                
                userdata ud = new userdata();
                ud.userid = user.Id;
                db.userdatas.Add(ud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        // GET: /AccountLogin/Edit/5
        public ActionResult EditLogin(int? id)
        {
            user user = db.users.Find(id);
            var role= db.user_role.Where(t => t.status == "active" && t.userid == id).FirstOrDefault();
            ViewBag.Role = role != null ? role.roleid : -1;
            return View(user);
        }
        public ActionResult SaveEditLogin(user model)
        {
            var user = db.users.Find(model.Id);
            db.Entry(user).State = EntityState.Modified;
            user.display = model.display;
            user.username = model.username;
            user.password = model.password;
            user.discount = model.discount;
            user.type = model.type;
           

            user.updatedate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditRole(int? id)
        {
            user user = db.users.Find(id);
            var userRole = db.user_role.FirstOrDefault(m => m.userid == user.Id);
            if (userRole!=null)
            {
                return View(userRole);
            }
            return null;
        }
        public ActionResult SaveEditRole(user_role model)
        {
            var user = db.user_role.Find(model.Id);
            user.roleid = model.roleid;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditInfo(int? id)
        {
            user user = db.users.Find(id);
            var userInfo = db.userdatas.Where(m => m.userid == id).FirstOrDefault();
            return View(userInfo);
        }
        public ActionResult SaveEditInfo(userdata model)
        {
            var user = db.userdatas.Find(model.Id);
            if (user!=null)
            {
                user.billing_address1 = model.billing_address1;
                user.billing_address2 = model.billing_address2;
                user.billing_country = model.billing_country;
                user.billing_email = model.billing_email;
                user.billing_name = model.billing_name;
                user.billing_phone = model.billing_phone;
                user.billing_poscode = model.billing_poscode;
                user.billing_state = model.billing_state;
                user.billing_suburb = model.billing_state;
                user.company_name = model.company_name;
                user.contact_email = model.contact_email;
                user.contact_phone = model.contact_phone;
                user.delivery_address1 = model.delivery_address1;
                user.delivery_address2 = model.delivery_address2;
                user.delivery_contry = model.delivery_contry;
                user.delivery_email = model.delivery_email;
                user.delivery_name = model.delivery_name;
                user.delivery_phone = model.delivery_phone;
                user.delivery_postcode = model.delivery_postcode;
                user.delivery_state = model.delivery_state;
                user.delivery_suburb = model.delivery_suburb;
                user.firstname = model.firstname;
                user.lasname = model.lasname;
                db.SaveChanges();
            }
         
            return RedirectToAction("Index");
        }

        public ActionResult YourProfile()
        {
            WebApplication1.Models.AllLoggedUserInfo userFullInfo = (WebApplication1.Models.AllLoggedUserInfo)Session["LoggedAccount"];
            return View(userFullInfo);
        }
        // POST: /AccountLogin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]      
        public ActionResult Edit(user model)
        {
            var user = db.users.Find( model.Id);
                db.Entry(user).State=EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            var user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /AccountLogin/Delete/5
        [HttpPost, ActionName("Delete")]
       
        public ActionResult DeleteConfirmed(long id)
        {
           // accountlogin accountlogin = db.accountlogins.Find(id);
            //db.accountlogins.Remove(accountlogin);
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
