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
    public class AdminController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }
        public ActionResult ProductDetail()
        {
            return View();
        }
        public ActionResult Logo()
        {
            return View();
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
