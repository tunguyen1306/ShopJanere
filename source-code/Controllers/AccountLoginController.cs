﻿using System;
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


        // GET: /AccountLogin/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            accountlogin accountlogin = db.accountlogins.Find(id);
            if (accountlogin == null)
            {
                return HttpNotFound();
            }
            return View(accountlogin);
        }

        // GET: /AccountLogin/Create
        public ActionResult Create()
        {
            user user = new user();
            return View(user);
        }

        // POST: /AccountLogin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,display,username,password")] user user)
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            AllLoggedUserInfo userfull = new AllLoggedUserInfo(user);
            return View(userfull);
        }

        // POST: /AccountLogin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AllLoggedUserInfo model)
        {
            var user = db.users.Where(m => m.Id == model.user.Id).FirstOrDefault();
                db.SaveChanges();
                return View(model);
        }

        // GET: /AccountLogin/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            accountlogin accountlogin = db.accountlogins.Find(id);
            if (accountlogin == null)
            {
                return HttpNotFound();
            }
            return View(accountlogin);
        }

        // POST: /AccountLogin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            accountlogin accountlogin = db.accountlogins.Find(id);
            db.accountlogins.Remove(accountlogin);
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
