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
        public ActionResult SeoTag(int idPage)
        {
            var seo = db.seos.FirstOrDefault(x => x.type == 2);
            if (seo != null)
            {
                return View(seo);
            }
            else
            {
                return View(new seo());
            }


        }
        [HttpPost]
        public ActionResult SeoTag(seo model)
        {
            if (model.id != 0)
            {
                var se = db.seos.Find(model.id);
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
