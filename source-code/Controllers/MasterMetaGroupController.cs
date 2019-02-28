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
            var listMetagroup =db.metagrups.Where(x=>x.IsActive==true && x.PARENTNO==0 && x.CodeLanguage == "english").Select(x=>new AllModel {tblMasterMetaGroup = x}).ToList();

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

                var matagroup = new metagrup();
                matagroup.PARENTNO = 0;
                matagroup.IsActive = model.tblMasterMetaGroup.IsActive;
                matagroup.CREATED = DateTime.Now;
                matagroup.METAGROUPNAME = model.tblMasterMetaGroupArray[0].METAGROUPNAME;
                matagroup.METAGROUPCODE = model.tblMasterMetaGroupArray[0].METAGROUPCODE;
                matagroup.CodeLanguage = model.tblMasterMetaGroupArray[0].CodeLanguage;
                db.metagrups.Add(matagroup);
                db.SaveChanges();
                var updatematagroup = db.metagrups.Find(matagroup.METAGROUPNO);
                updatematagroup.IdCurrentItem = matagroup.METAGROUPNO;
                db.Entry(updatematagroup).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in model.tblMasterMetaGroupArray.Skip(1).ToList())
                {

                    item.PARENTNO = 0;
                    item.IsActive = model.tblMasterMetaGroup.IsActive;
                    item.CREATED = DateTime.Now;
                    item.IdCurrentItem = updatematagroup.IdCurrentItem;
                    db.metagrups.Add(item);

                }
                db.SaveChanges();
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = matagroup.IdCurrentItem + "." +
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
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int code)
        {
           
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPNO == code);

            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.metagrups.FirstOrDefault(x => x.IdCurrentItem == code && x.CodeLanguage == itemLang.language.ToLower());
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
                        tblItem.IdCurrentItem = proMaster.IdCurrentItem;
                        db.metagrups.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var listCode = list.Select(y => y.language.ToLower()).ToList();
           
            var item = db.metagrups.ToList().Where(x => x.IdCurrentItem == code && listCode.Contains(x.CodeLanguage)).ToList();
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
                                var fname = item.IdCurrentItem + "." +
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
            var list = db.metagrups.Where(x=>x.IdCurrentItem==id).ToList();
            foreach (var item in list)
            {
                metagrup metagrup = db.metagrups.Find(item.METAGROUPNO);
                metagrup.IsActive = false;
                db.Entry(metagrup).State = EntityState.Modified;
            }
          
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

                    var list = db.metagrups.Where(x => x.IdCurrentItem == item.METAGROUPNO).ToList();
                    foreach (var item1 in list)
                    {
                        metagrup metagrup = db.metagrups.Find(item1.METAGROUPNO);
                        metagrup.IsActive = false;
                        db.Entry(metagrup).State = EntityState.Modified;
                    }
                   
                   
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

                    var list = db.artgrps.Where(x => x.IdCurrentItem == item.GROUPNO).ToList();
                    foreach (var item1 in list)
                    {
                        artgrp metagrup = db.artgrps.Find(item1.GROUPNO);
                        metagrup.IsActive = false;
                        db.Entry(metagrup).State = EntityState.Modified;
                    }


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
                           
                                 select new AllModel { tblMetaGroup = cat ,tblMasterMetaGroup = catl }).Where(x => x.tblMetaGroup.IsActive == true && x.tblMetaGroup.CodeLanguage == "english"  && x.tblMetaGroup.PARENTNO!=0).ToList();
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
            listMasterMetaGroup.Insert(0,new metagrup {IdCurrentItem = 0,METAGROUPNAME = "Select Master Meta Group" });
            ViewBag.ListMasterMetaGroup = listMasterMetaGroup;
            return View(new AllModel { tblMetaGroup = new metagrup() });
        }

        [HttpPost]
        public ActionResult CreateMetaGroup(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblMetaGroupArray != null)
            {

                var matagroup = new metagrup();
                matagroup.PARENTNO = model.tblMetaGroup.PARENTNO;
                matagroup.IsActive = model.tblMetaGroup.IsActive;
                matagroup.CREATED = DateTime.Now;
                matagroup.METAGROUPNAME = model.tblMetaGroupArray[0].METAGROUPNAME;
                matagroup.METAGROUPCODE = model.tblMetaGroupArray[0].METAGROUPCODE;
                matagroup.CodeLanguage ="english";
                db.metagrups.Add(matagroup);
                db.SaveChanges();
                var updatematagroup = db.metagrups.Find(matagroup.METAGROUPNO);
                updatematagroup.IdCurrentItem = matagroup.METAGROUPNO;
                db.Entry(updatematagroup).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in model.tblMetaGroupArray.Skip(1).ToList())
                {

                    item.PARENTNO = model.tblMetaGroup.PARENTNO;
                    item.IsActive = model.tblMetaGroup.IsActive;
                    item.CREATED = DateTime.Now;
                    item.IdCurrentItem = updatematagroup.IdCurrentItem;
                    db.metagrups.Add(item);

                }
                db.SaveChanges();
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = matagroup.IdCurrentItem + "." +
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
       
            return RedirectToAction("IndexMetaGroup");

        }

        public ActionResult EditMetaGroup(int code)
        {
            var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO == 0).ToList();
            listMasterMetaGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Master Meta Group" });
            ViewBag.ListMasterMetaGroup = listMasterMetaGroup;
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPNO == code);
          
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.metagrups.FirstOrDefault(x => x.IdCurrentItem == code && x.CodeLanguage == itemLang.language.ToLower());
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
                        tblItem.IdCurrentItem = proMaster.IdCurrentItem;
                        db.metagrups.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var listCode = list.Select(y => y.language.ToLower()).ToList();

            var item = db.metagrups.ToList().Where(x => x.IdCurrentItem == code && listCode.Contains(x.CodeLanguage)).ToList();
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
                    tem.PARENTNO = model.tblMetaGroup.PARENTNO;
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
                                var fname = item.IdCurrentItem + "." +
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

            var list = db.metagrups.Where(x => x.IdCurrentItem == id).ToList();
            foreach (var item in list)
            {
                metagrup metagrup = db.metagrups.Find(item.METAGROUPNO);
                metagrup.IsActive = false;
                db.Entry(metagrup).State = EntityState.Modified;
            }

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
            return View(new AllModel { tblGroup = new artgrp() });
        }

        [HttpPost]
        public ActionResult CreateGroup(AllModel model, HttpPostedFileBase[] inputfile)
        {

            if (model.tblGroupArray != null)
            {


                var groupF = new artgrp();

                groupF.METAGROUPNO = model.tblGroup.METAGROUPNO;
                groupF.IsActive = model.tblGroup.IsActive;
                groupF.CREATED = DateTime.Now;
                groupF.LASTCHANGE = DateTime.Now;
                groupF.EXPORTABLE = "T";
                groupF.CodeLanguage = model.tblGroupArray[0].CodeLanguage.ToLower();
                groupF.GROUPCODE = model.tblGroupArray[0].GROUPCODE;
                groupF.GROUPNAME = model.tblGroupArray[0].GROUPNAME;
                db.artgrps.Add(groupF);
                db.SaveChanges();
                var updategroupF = db.artgrps.Find(groupF.GROUPNO);
                updategroupF.IdCurrentItem = groupF.GROUPNO;
                db.Entry(updategroupF).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in model.tblGroupArray.Skip(1).ToList())
                {
                   
                        item.METAGROUPNO = model.tblGroup.METAGROUPNO;
                        item.IsActive = model.tblGroup.IsActive;
                        item.CREATED = DateTime.Now;
                        item.LASTCHANGE = DateTime.Now;
                        item.EXPORTABLE = "T";
                        item.IdCurrentItem = groupF.IdCurrentItem;
                        db.artgrps.Add(item);
                    

                }
                db.SaveChanges();
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {
                            var fname = "" + "." +
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

  

           
            return RedirectToAction("IndexGroup");

        }

        public ActionResult EditGroup(int id)
        {
            var listMasterGroup = db.metagrups.Where(x => x.PARENTNO != 0).ToList();
            listMasterGroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.ListMasterGroup = listMasterGroup;
            var proMaster = db.artgrps.FirstOrDefault(x => x.GROUPNO == id);
           
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.artgrps.FirstOrDefault(x => x.IdCurrentItem == id && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var tblItem = new artgrp();
                        tblItem.CREATED = proMaster.CREATED;
                        tblItem.LASTCHANGE = proMaster.LASTCHANGE;
                        tblItem.GROUPCODE = proMaster.GROUPCODE;
                        tblItem.GROUPNAME = proMaster.GROUPNAME;
                        tblItem.IsActive = proMaster.IsActive;
                        tblItem.EXPORTABLE = proMaster.EXPORTABLE;
                        tblItem.METAGROUPNO = proMaster.METAGROUPNO;
                        tblItem.CodeLanguage = itemLang.language.ToLower();
                        tblItem.IdCurrentItem = proMaster.IdCurrentItem;
                        db.artgrps.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var listCode = list.Select(y => y.language.ToLower()).ToList();
            var item = db.artgrps.ToList().Where(x => x.IdCurrentItem == id && listCode.Contains(x.CodeLanguage)).ToList();
            return View(new AllModel { listGroup = item, tblGroup = proMaster });
        }
        [HttpPost]
        public ActionResult EditGroup(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblGroupArray != null)
            {
                foreach (var item in model.tblGroupArray)
                {
                    var tem = db.artgrps.Find(item.GROUPNO);

                    tem.IsActive = model.tblGroup.IsActive;
                    tem.GROUPCODE = item.GROUPCODE;
                    tem.GROUPNAME = item.GROUPNAME;
                    tem.METAGROUPNO = model.tblGroup.METAGROUPNO;
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
                                var fname = item.GROUPCODE + "." +
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



            return RedirectToAction("IndexGroup");
        }

        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.artgrps.Where(x => x.IdCurrentItem == id).ToList();
            foreach (var item in list)
            {
                artgrp artgrp = db.artgrps.Find(item.GROUPNO);
                artgrp.IsActive = false;
                db.Entry(artgrp).State = EntityState.Modified;
            }

            db.SaveChanges();
           
            return RedirectToAction("IndexGroup");
        }

        #endregion

    }
}
