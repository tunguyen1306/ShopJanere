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
    public class SettingController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /setting/
        public ActionResult Index(string type=null)
        {
            var list = db.settingtypes.Where(x => x.status == 1 && x.code!="paypal").ToList();
            list.Insert(0, new settingtype { id = 0, name = "Type" });
            ViewBag.ListTypeSetting = list;
            ViewBag.Type = type;
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10, string type = null)
        {
            var listsetting = db.settings.Where(x=>x.typeId!= "paypal").Join(db.settingtypes, st => st.typeId, stt => stt.name, (st, stt) => new AllModel { tblSetting = st, tblSettingType = stt }).ToList();
            if (type != null && type=="paypal")
            {
                listsetting = db.settings.Where(x => x.typeId == "paypal").Join(db.settingtypes, st => st.typeId, stt => stt.name, (st, stt) => new AllModel { tblSetting = st, tblSettingType = stt }).ToList();

            }
            if (type != null && type != "paypal")
            {
                listsetting = listsetting.Where(x => x.tblSetting.typeId == type).ToList();

            }
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listsetting.Count;
            listsetting = listsetting.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listsetting.Count;

            return PartialView(listsetting);
        }
        // GET: /setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting setting = db.settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: /setting/Create
        public ActionResult Create()
        {
            var list = db.settingtypes.Where(x => x.status == 1).ToList();
            ViewBag.ListTypeSetting = list;
            var listMetagrup = db.metagrups.ToList();
            // listMetagrup.Insert(0,new metagrup {METAGROUPNO = 0,METAGROUPNAME = "Select"});
            ViewBag.ListGroup = listMetagrup;
            
            return View(new setting());
        }

        // POST: /setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(setting setting, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != null)
                        {

                            string path = "";
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(files[i].FileName));
                            var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                            files[i].SaveAs(path);
                            setting.urlimage = "/Content/ProductImage/" + files[i].FileName;


                        }
                    }
                }
                db.settings.Add(setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        // GET: /setting/Edit/5
        public ActionResult Edit(int? id)
        {
            var list = db.settingtypes.Where(x => x.status == 1).ToList();
            list.Insert(0, new settingtype { id = 0, name = "Type" });
            ViewBag.ListTypeSetting = list;
            var listMetagrup = db.metagrups.ToList();
            //listMetagrup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select" });
            ViewBag.ListGroup = listMetagrup;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting setting = db.settings.Find(id);
            if (setting == null)
            {
                
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: /setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(setting setting, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != null)
                        {

                            string path = "";
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(files[i].FileName));
                            var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                            files[i].SaveAs(path);
                            setting.urlimage = "/Content/ProductImage/" + files[i].FileName;


                        }
                    }
                }
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: /setting/Delete/5
        public ActionResult Delete(int? id)
        {
            setting setting = db.settings.Find(id);
            db.settings.Remove(setting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /setting/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    setting setting = db.settings.Find(id);
        //    db.settings.Remove(setting);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
