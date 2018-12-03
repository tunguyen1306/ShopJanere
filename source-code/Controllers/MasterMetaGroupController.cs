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
    public class MasterMetaGroupController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexAjax(string search=null,int start = 0, int view = 10)
        {
            var listMetagroup = (from cat in db.metagrups
                                 //join catl in db.metagrups on cat.MetaGroupId equals catl.METAGROUPNO, tblMetaGroup = catl
                                 select new AllModel { tblMetaGroup = cat }).Where(x=>x.tblMetaGroup.IsActive==true && x.tblMetaGroup.PARENTNO == 0).ToList();

            if (search!=null)
            {
                listMetagroup =listMetagroup.Where(x => x.tblMetaGroup.METAGROUPNAME.ToLower().Contains(search.ToLower())).ToList();
            }

            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listMetagroup.Count;
            listMetagroup = listMetagroup.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listMetagroup.Count;

            return PartialView(listMetagroup);
        }

        public ActionResult Create()
        {
            return View(new metagrup());
        }

        [HttpPost]
        public ActionResult Create( metagrup metagrup, HttpPostedFileBase[] inputfile)
        {
            var mmgroup = db.metagrups.OrderByDescending(x => x.METAGROUPNO).FirstOrDefault();
            if (mmgroup != null)
            {
                metagrup.METAGROUPNO = mmgroup.METAGROUPNO + 1;
            }
            else
            {
                metagrup.METAGROUPNO = 1;
            }
            if (inputfile != null)
            {
                for (int i = 0; i < inputfile.Length; i++)
                {
                    if (inputfile[i] != null)
                    {
                        var fname = metagrup .METAGROUPCODE+ "." +
                                  inputfile[i].FileName.Split('.').Last();
                        string path = Server.MapPath("~/Content/MetagroupImage");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        path = Path.Combine(path + "/" , fname);
                        inputfile[i].SaveAs(path);

                    }
                }
            }
            metagrup.PARENTNO = 0;
            metagrup.CREATED = DateTime.Now;
                db.metagrups.Add(metagrup);
                db.SaveChanges();
                return RedirectToAction("Index");

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metagrup metagrup = db.metagrups.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }
        [HttpPost]
        public ActionResult Edit(metagrup metagrup, HttpPostedFileBase[] inputfile)
        {
            if (ModelState.IsValid)
            {
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = metagrup.METAGROUPCODE + "." +
                                      inputfile[i].FileName.Split('.').Last();
                            string path = Server.MapPath("~/Content/MetagroupImage");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            path = Path.Combine(path + "/", fname);
                            inputfile[i].SaveAs(path);

                        }
                    }
                }
                metagrup.LASTCHANGE = DateTime.Now;
                metagrup.PARENTNO = 0;
                db.Entry(metagrup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(metagrup);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            metagrup metagrup = db.metagrups.Find(id);
            metagrup.IsActive = false;
            db.Entry(metagrup).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteImg(string code=null)
        {

            if (code!=null)
            {
                try
                {
                    string dir = Server.MapPath("~/Content/MetagroupImage/" + code + ".jpg" );
                    System.IO.File.Delete(dir);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return Json("");
        }
      
        #region
        public ActionResult IndexMetaGroup()
        {
            return View();
        }

        public ActionResult IndexAjaxMetaGroup(string search=null,int start = 0, int view = 10)
        {
            var listMetagroup = (from cat in db.metagrups
                                    join catl in db.metagrups on cat.PARENTNO equals catl.METAGROUPNO
                                 select new AllModel { tblMetaGroup = cat ,tblMasterMetaGroup = catl }).Where(x => x.tblMetaGroup.IsActive == true && x.tblMetaGroup.PARENTNO!=0).ToList();
            if (search != null)
            {
                listMetagroup = listMetagroup.Where(x => x.tblMetaGroup.METAGROUPNAME.ToLower().Contains(search.ToLower())).ToList();
            }


            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listMetagroup.Count;
            listMetagroup = listMetagroup.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listMetagroup.Count;

            return PartialView(listMetagroup);
        }

        public ActionResult CreateMetaGroup()
        {
            var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO == 0).ToList();
            listMasterMetaGroup.Insert(0,new metagrup {METAGROUPNO = 0,METAGROUPNAME = "Select Master Meta Group" });
            ViewBag.ListMasterMetaGroup = listMasterMetaGroup;
            return View(new metagrup());
        }

        [HttpPost]
        public ActionResult CreateMetaGroup(metagrup metagrup, HttpPostedFileBase[] inputfile)
        {
            var mmgroup = db.metagrups.OrderByDescending(x => x.METAGROUPNO).FirstOrDefault();
            if (mmgroup != null)
            {
                metagrup.METAGROUPNO = mmgroup.METAGROUPNO + 1;
            }
            else
            {
                metagrup.METAGROUPNO = 1;
            }
            if (inputfile != null)
            {
                for (int i = 0; i < inputfile.Length; i++)
                {
                    if (inputfile[i] != null)
                    {
                        var fname = metagrup.METAGROUPCODE + "." +
                                  inputfile[i].FileName.Split('.').Last();
                        string path = Server.MapPath("~/Content/MetagroupImage");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        path = Path.Combine(path + "/", fname);
                        inputfile[i].SaveAs(path);

                    }
                }
            }
            metagrup.PARENTNO = 0;
            metagrup.CREATED = DateTime.Now;
            db.metagrups.Add(metagrup);
            db.SaveChanges();
            return RedirectToAction("IndexMetaGroup");

        }

        public ActionResult EditMetaGroup(int? id)
        {
            var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO == 0).ToList();
            listMasterMetaGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Master Meta Group" });
            ViewBag.ListMasterMetaGroup = listMasterMetaGroup;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metagrup metagrup = db.metagrups.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }
        [HttpPost]
        public ActionResult EditMetaGroup(metagrup metagrup, HttpPostedFileBase[] inputfile)
        {
            if (ModelState.IsValid)
            {
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = metagrup.METAGROUPCODE + "." +
                                      inputfile[i].FileName.Split('.').Last();
                            string path = Server.MapPath("~/Content/MetagroupImage");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            path = Path.Combine(path + "/", fname);
                            inputfile[i].SaveAs(path);

                        }
                    }
                }
                metagrup.LASTCHANGE = DateTime.Now;
              
                db.Entry(metagrup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexMetaGroup");
            }
            return View(metagrup);
        }

        public ActionResult DeleteMetaGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            metagrup metagrup = db.metagrups.Find(id);
            metagrup.IsActive = false;
            db.Entry(metagrup).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexMetaGroup");
        }

        #endregion

        #region
        public ActionResult IndexGroup()
        {
            return View();
        }

        public ActionResult IndexAjaxGroup(string search = null, int start = 0, int view = 10)
        {
            var listGroup = (from cat in db.artgrps
                             join catl in db.metagrups on cat.METAGROUPNO equals catl.METAGROUPNO
                             select new AllModel { tblGroup = cat, tblMetaGroup = catl }).ToList();
            if (search != null)
            {
                listGroup = listGroup.Where(x => x.tblGroup.GROUPNAME.ToLower().Contains(search.ToLower())).ToList();
            }


            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listGroup.Count;
            listGroup = listGroup.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listGroup.Count;

            return PartialView(listGroup);
        }

        public ActionResult CreateGroup()
        {
            var listMasterGroup = db.metagrups.Where(x => x.PARENTNO != 0).ToList();
            listMasterGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.ListMasterGroup = listMasterGroup;
            return View(new artgrp());
        }

        [HttpPost]
        public ActionResult CreateGroup(artgrp metagrup, HttpPostedFileBase[] inputfile)
        {
            var mmgroup = db.artgrps.OrderByDescending(x => x.GROUPNO).FirstOrDefault();
            if (mmgroup != null)
            {
                metagrup.GROUPNO = mmgroup.GROUPNO + 1;
            }
            else
            {
                metagrup.GROUPNO = 1;
            }
            if (inputfile != null)
            {
                for (int i = 0; i < inputfile.Length; i++)
                {
                    if (inputfile[i] != null)
                    {
                        var fname = metagrup.GROUPCODE + "." +
                                  inputfile[i].FileName.Split('.').Last();
                        string path = Server.MapPath("~/Content/GroupImage");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        path = Path.Combine(path + "/", fname);
                        inputfile[i].SaveAs(path);

                    }
                }
            }
            
            metagrup.CREATED = DateTime.Now;
            metagrup.LASTCHANGE = DateTime.Now;
            db.artgrps.Add(metagrup);
            db.SaveChanges();
            return RedirectToAction("IndexGroup");

        }

        public ActionResult EditGroup(int? id)
        {
            var listMasterGroup = db.metagrups.Where(x => x.PARENTNO != 0).ToList();
            listMasterGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.ListMasterGroup = listMasterGroup;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            artgrp metagrup = db.artgrps.Find(id);
            if (metagrup == null)
            {
                return HttpNotFound();
            }
            return View(metagrup);
        }
        [HttpPost]
        public ActionResult EditGroup(artgrp metagrup, HttpPostedFileBase[] inputfile)
        {
            if (ModelState.IsValid)
            {
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = metagrup.GROUPCODE + "." +
                                      inputfile[i].FileName.Split('.').Last();
                            string path = Server.MapPath("~/Content/GroupImage");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            path = Path.Combine(path + "/", fname);
                            inputfile[i].SaveAs(path);

                        }
                    }
                }
                metagrup.LASTCHANGE = DateTime.Now;
                metagrup.LASTCHANGE = DateTime.Now;
                db.Entry(metagrup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexGroup");
            }
            return View(metagrup);
        }

        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            metagrup metagrup = db.metagrups.Find(id);
            metagrup.IsActive = false;
            db.Entry(metagrup).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexGroup");
        }

        #endregion

    }
}
