using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SeoController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

       public ActionResult GoogleTag()
       {
           var seo = db.seos.FirstOrDefault(x => x.type == 1);
           if (seo!=null)
           {
                return View(seo);
            }
           else
           {
                return View(new seo());
            }
           

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GoogleTag(seo model)
        {
            if (model.id!=0)
            {
                var se = db.seos.Find(model.id);
                se.googleanalys = model.googleanalys;
                se.description = model.description;
                se.metatag = model.metatag;
                se.type = model.type;
                se.page = model.page;
                se.link = model.link;
                se.keyword = model.keyword;
                se.title = model.title;
                db.Entry(se).State=EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.seos.Add(model);
                db.SaveChanges();
            }
            return View(model);
        }
        public ActionResult SeoTag(string page)
        {
            var seo = db.seos.FirstOrDefault(x => x.type == 2 && x.page== page);
            if (seo != null)
            {
                return View(seo);
            }
            else
            {
                return View(new seo());
            }


        }
        public JsonResult GetSeoTag(string page)
        {
            var seo = db.seos.FirstOrDefault(x => x.type == 2 && x.page == page);
            if (seo != null)
            {
                return Json(new { result = seo },JsonRequestBehavior.AllowGet);
            }
            return Json(new { result= "" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SeoTag(seo model)
        {
            if (model.id != 0)
            {
                var se = db.seos.Find(model.id);
                se.googleanalys = model.googleanalys;
                se.description = model.description;
                se.metatag = model.metatag;
                se.type = model.type;
                se.page = model.page;
                se.link = model.link;
                se.keyword = model.keyword;
                se.title = model.title;
                se.title = model.title;
                db.Entry(se).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.seos.Add(model);
                db.SaveChanges();
            }
            return View(model);
           
        }

    }
}
