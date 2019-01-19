﻿using System;
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
            var listMetagroup =db.metagrups.Where(x=>x.IsActive==true && x.PARENTNO==0).Select(x=>new AllModel {tblMasterMetaGroup = x}).ToList();

            if (search!=null)
            {
                listMetagroup =listMetagroup.Where(x => x.tblMasterMetaGroup.METAGROUPNAME.ToLower().Contains(search.ToLower())).ToList();
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
            return View(new AllModel { tblMasterMetaGroup = new metagrup()});
        }

        [HttpPost]
        public ActionResult Create( AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblMasterMetaGroupArray != null)
            {
                foreach (var item in model.tblMasterMetaGroupArray)
                {
                    
                        item.PARENTNO = 0;
                        item.IsActive = model.tblMasterMetaGroup.IsActive;
                        item.CREATED = DateTime.Now;
                        db.metagrups.Add(item);
                        db.SaveChanges();
                        if (inputfile != null)
                        {
                            for (int i = 0; i < inputfile.Length; i++)
                            {
                                if (inputfile[i] != null)
                                {
                                    var fname = item.METAGROUPCODE + "." +
                                              inputfile[i].FileName.Split('.').Last();
                                    string path = Server.MapPath("~/Content/MetagroupImage");
                                    if (!Directory.Exists(path))
                                        Directory.CreateDirectory(path);
                                    path = Path.Combine(path + "/", fname);
                                    inputfile[i].SaveAs(path);

                                }
                            }
                        }
                    

                }
            }
                return RedirectToAction("Index");

        }

        public ActionResult Edit(string code)
        {
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPCODE == code);
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.countries.Where(x => x.status == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.metagrups.FirstOrDefault(x => x.METAGROUPCODE == code && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var tblItem = new metagrup();
                        tblItem.CREATED = proMaster.CREATED;
                        tblItem.LASTCHANGE = proMaster.LASTCHANGE;
                        tblItem.METAGROUPCODE = proMaster.METAGROUPCODE;
                        tblItem.METAGROUPNAME = proMaster.METAGROUPNAME;
                        tblItem.IsActive = proMaster.IsActive;
                        tblItem.PARENTNO = proMaster.PARENTNO;
                        tblItem.CodeLanguage = itemLang.language.ToLower();
                        db.metagrups.Add(tblItem);
                        
                    }

                }

            }
            db.SaveChanges();
            var item = db.metagrups.ToList().Where(x => x.METAGROUPCODE.ToLower() == code.ToLower()).ToList();
            return View(new AllModel { listMasterMetaGroup = item, tblMasterMetaGroup = proMaster });
        }
        [HttpPost]
        public ActionResult Edit(AllModel model, HttpPostedFileBase[] inputfile)
        {

            if (model.tblMasterMetaGroupArray != null)
            {
                foreach (var item in model.tblMasterMetaGroupArray)
                {
                    var tem = db.metagrups.Find(item.METAGROUPNO);

                    tem.IsActive = model.tblMasterMetaGroup.IsActive;
                    tem.METAGROUPCODE = item.METAGROUPCODE;
                    tem.METAGROUPNAME = item.METAGROUPNAME;
                    tem.PARENTNO = 0;
                    tem.CREATED = item.CREATED;
                    tem.LASTCHANGE = item.LASTCHANGE;
                    tem.CodeLanguage = item.CodeLanguage.ToLower();
                    db.Entry(tem).State = EntityState.Modified;
                    if (inputfile != null)
                    {
                        for (int i = 0; i < inputfile.Length; i++)
                        {
                            if (inputfile[i] != null)
                            {
                                var fname = item.METAGROUPCODE + "." +
                                          inputfile[i].FileName.Split('.').Last();
                                string path = Server.MapPath("~/Content/MetagroupImage");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                path = Path.Combine(path + "/", fname);
                                inputfile[i].SaveAs(path);

                            }
                        }
                    }

                }

            }

            db.SaveChanges();


            return RedirectToAction("Index");

           
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
        public JsonResult UpdateStatus(string status, string[] ids)
        {
            var _ids = ids.Select(t => int.Parse(t)).ToList();
            bool _status = false;
            var msg = "Update Success";
            try
            {
                var _items = db.metagrups.Where(x => _ids.Contains(x.METAGROUPNO));
                foreach (var item in _items)
                {
                    var meta = db.metagrups.Find(item.METAGROUPNO);
                    meta.IsActive = false;
                    db.Entry(meta).State = EntityState.Modified;
                   
                }
                db.SaveChanges();
                _status = true;
            }
            catch (Exception)
            {

                msg = "Update Failed";
            }
            return Json(new { status = _status, mgs = msg });
        }
        public JsonResult UpdateStatusGroup(string status, string[] ids)
        {
            var _ids = ids.Select(t => int.Parse(t)).ToList();
            bool _status = false;
            var msg = "Update Success";
            try
            {
                var _items = db.artgrps.Where(x => _ids.Contains(x.GROUPNO));
                foreach (var item in _items)
                {
                    var meta = db.artgrps.Find(item.GROUPNO);
                    meta.IsActive = false;
                    db.Entry(meta).State = EntityState.Modified;
                   
                }
                db.SaveChanges();
                _status = true;
            }
            catch (Exception)
            {

                msg = "Update Failed";
            }
            return Json(new { status = _status, mgs = msg });
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
            return View(new AllModel { tblMasterMetaGroup = new metagrup() });
        }

        [HttpPost]
        public ActionResult CreateMetaGroup(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblMasterMetaGroupArray != null)
            {
                foreach (var item in model.tblMasterMetaGroupArray)
                {

                    item.PARENTNO = 0;
                    item.IsActive = model.tblMasterMetaGroup.IsActive;
                    item.CREATED = DateTime.Now;
                    db.metagrups.Add(item);
                    db.SaveChanges();
                    if (inputfile != null)
                    {
                        for (int i = 0; i < inputfile.Length; i++)
                        {
                            if (inputfile[i] != null)
                            {
                                var fname = item.METAGROUPCODE + "." +
                                          inputfile[i].FileName.Split('.').Last();
                                string path = Server.MapPath("~/Content/MetagroupImage");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                path = Path.Combine(path + "/", fname);
                                inputfile[i].SaveAs(path);

                            }
                        }
                    }


                }
            }
       
            return RedirectToAction("IndexMetaGroup");

        }

        public ActionResult EditMetaGroup(string code)
        {
            var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO == 0).ToList();
            listMasterMetaGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Master Meta Group" });
            ViewBag.ListMasterMetaGroup = listMasterMetaGroup;
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPCODE == code);
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.countries.Where(x => x.status == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.metagrups.FirstOrDefault(x => x.METAGROUPCODE == code && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var tblItem = new metagrup();
                        tblItem.CREATED = proMaster.CREATED;
                        tblItem.LASTCHANGE = proMaster.LASTCHANGE;
                        tblItem.METAGROUPCODE = proMaster.METAGROUPCODE;
                        tblItem.METAGROUPNAME = proMaster.METAGROUPNAME;
                        tblItem.IsActive = proMaster.IsActive;
                        tblItem.PARENTNO = proMaster.METAGROUPNO;
                        tblItem.CodeLanguage = itemLang.language.ToLower();
                        db.metagrups.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var item = db.metagrups.ToList().Where(x => x.METAGROUPCODE.ToLower() == code.ToLower()).ToList();
            return View(new AllModel { listMetaGroup = item, tblMetaGroup = proMaster });
        }
        [HttpPost]
        public ActionResult EditMetaGroup(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblMetaGroupArray != null)
            {
                foreach (var item in model.tblMetaGroupArray)
                {
                    var tem = db.metagrups.Find(item.METAGROUPNO);

                    tem.IsActive = model.tblMetaGroup.IsActive;
                    tem.METAGROUPCODE = item.METAGROUPCODE;
                    tem.METAGROUPNAME = item.METAGROUPNAME;
                    tem.PARENTNO = 0;
                    tem.CREATED = item.CREATED;
                    tem.LASTCHANGE = item.LASTCHANGE;
                    tem.CodeLanguage = item.CodeLanguage.ToLower();
                    db.Entry(tem).State = EntityState.Modified;
                    if (inputfile != null)
                    {
                        for (int i = 0; i < inputfile.Length; i++)
                        {
                            if (inputfile[i] != null)
                            {
                                var fname = item.METAGROUPCODE + "." +
                                          inputfile[i].FileName.Split('.').Last();
                                string path = Server.MapPath("~/Content/MetagroupImage");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                path = Path.Combine(path + "/", fname);
                                inputfile[i].SaveAs(path);

                            }
                        }
                    }

                }

            }

            db.SaveChanges();


         
            return RedirectToAction("IndexMetaGroup");
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
                             where cat.IsActive==true
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
                metagrup.CREATED = DateTime.Now;
                metagrup.LASTCHANGE = DateTime.Now;
                metagrup.EXPORTABLE ="T";
                
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
