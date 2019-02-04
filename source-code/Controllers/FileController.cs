using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/
        private veebdbEntities db = new veebdbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listStore = db.files.Where(m => m.Status == "Active").ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listStore.Count;
            listStore = listStore.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listStore.Count;

            return PartialView(listStore);
        }
        // GET: /Containt/Details/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            file file = db.files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }
        [HttpGet]
        // GET: /Containt/Create
        public ActionResult Create()
        {
            ViewBag.Filepath = Path.Combine(Server.MapPath("/UploadFile"));
            return View(new AllModel { tblFile = new file() });
        }


        [HttpPost]

        public ActionResult Create(AllModel model, HttpPostedFileBase inputfile)
        {
            //  if (ModelState.IsValid)
            // {
            try
            {
                //save file
                if (model.tblFileArray != null && inputfile.ContentLength > 0)
                {
                    string path = "";
                    try
                    {
                        path = Path.Combine(Server.MapPath("/UploadFile"), Path.GetFileName(inputfile.FileName));

                        inputfile.SaveAs(path);
                        //save other infomation

                        var matagroup = new file();
                        matagroup.FileName = inputfile.FileName;
                        matagroup.Status = "Active";
                        matagroup.FileAddress = path;
                        matagroup.Title = model.tblFileArray[0].Title;
                        matagroup.Description = model.tblFileArray[0].Description;
                        matagroup.CodeLanguage = model.tblFileArray[0].CodeLanguage;
                        matagroup.UploadDate = DateTime.Now;
                        matagroup.UploadBy = User.Identity.Name;
                        matagroup.Kind = inputfile.ContentType;
                        db.files.Add(matagroup);
                        db.SaveChanges();
                        var updatematagroup = db.files.Find(matagroup.Id);
                        updatematagroup.IdCurrentItem = matagroup.Id;
                        db.Entry(updatematagroup).State = EntityState.Modified;
                        db.SaveChanges();
                        foreach (var item in model.tblFileArray.Skip(1).ToList())
                        {

                            var matagroupL = new file();
                            matagroupL.FileName = item.FileName;
                            matagroupL.Status = "Active";
                            matagroupL.FileAddress = path;
                            matagroupL.Title = item.Title;
                            matagroupL.Description = item.Description;
                            matagroupL.CodeLanguage = item.CodeLanguage;
                            matagroupL.UploadDate = DateTime.Now;
                            matagroupL.UploadBy = User.Identity.Name;
                            matagroupL.Kind = inputfile.ContentType;
                            db.files.Add(matagroupL);

                        }
                        db.SaveChanges();


                        ViewBag.Message = "File uploaded successfully";
                        //save other infomation
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
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

        // GET: /Containt/Edit/5
        public ActionResult Edit(int? code)
        {

            var proMaster = db.files.FirstOrDefault(x => x.Id == code);

            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            foreach (var itemLang in list)
            {
                var pro = db.files.FirstOrDefault(x => x.IdCurrentItem == code && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proMaster != null)
                    {
                        var matagroupL = new file();
                        matagroupL.FileName = proMaster.FileName;
                        matagroupL.Status = "Active";
                        matagroupL.FileAddress = proMaster.FileAddress;
                        matagroupL.Title = proMaster.Title;
                        matagroupL.Description = proMaster.Description;
                        matagroupL.CodeLanguage = itemLang.language;
                        matagroupL.UploadDate = DateTime.Now;
                        matagroupL.UploadBy = proMaster.UploadBy;
                        matagroupL.Kind = proMaster.Kind;
                        matagroupL.IdCurrentItem = code;
                        db.files.Add(matagroupL);

                    }

                }

            }
            db.SaveChanges();
            var item = db.files.ToList().Where(x => x.IdCurrentItem == code).ToList();
            return View(new AllModel { listFile = item, tblFile = proMaster });

        }

        // POST: /Containt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AllModel model, HttpPostedFileBase inputfile)
        {

            foreach (var item in model.tblFileArray)
            {
                var tem = db.files.Find(item.Id);

                tem.FileName = item.FileName;
                tem.Status = "Active";
                tem.FileAddress = tem.FileAddress;
                tem.Title = item.Title;
                tem.Description = item.Description;
                tem.UploadDate = DateTime.Now;
                tem.UploadBy = tem.UploadBy;
                tem.Kind = tem.Kind;
                tem.CodeLanguage = item.CodeLanguage.ToLower();
                db.Entry(tem).State = EntityState.Modified;
                if (inputfile != null)
                {
                    string path = "";
                    try
                    {
                        path = Path.Combine(Server.MapPath("/UploadFile"), Path.GetFileName(inputfile.FileName));

                        inputfile.SaveAs(path);
                        //save other infomation
                        tem.FileName = inputfile.FileName;
                        tem.FileAddress = path;
                        tem.UploadDate = DateTime.Now;
                        tem.UploadBy = User.Identity.Name;
                        tem.Kind = inputfile.ContentType;
                        db.Entry(tem).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index");
                        //save other infomation
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
                else
                {
                
                    db.Entry(tem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }



            db.SaveChanges();


            return RedirectToAction("Index");
            //save file






        }

        public ActionResult FileDeletes(int[] id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Site");
            }
            //file file = data.files.Find(id);
            foreach (var item in id)
            {
                file file = db.files.Find(item);
                file.Status = "no";
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: /Containt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult FileDeleteConfirmed(int id)
        {
            file file = db.files.Find(id);
            file.Status = "deleted";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FileResult Download(int id)
        {

            file file = db.files.Find(id);
            //var FileVirtualPath = "~/UploadFile/" + file.FileName;
            var FileVirtualPath = Path.Combine(Server.MapPath("/UploadFile/"), Path.GetFileName(file.FileName));

            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }
        public ActionResult Downloads(int[] id)
        {

            var FileVirtualPath = System.Web.HttpContext.Current.Server.MapPath(@"~\UploadFile\");
            // string FileVirtualPath = ConfigurationManager.AppSettings["resources"] + "/" + ProjID + "/";
            try
            {
                if (id.Count() > 0)
                {
                    using (var _objzip = new ZipFile())
                    {
                        List<string> FilesNames = new List<string>();
                        foreach (int item in id)
                        {
                            FilesNames.Add(db.files.Find(item).FileName);
                        }
                        // var FileNames = files.Select(f => f.Name).ToList();
                        foreach (var item in FilesNames)
                        {
                            _objzip.AddFile(FileVirtualPath + item, "Files");
                        }
                        Random r = new Random();
                        Response.Clear();
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "download" + r.Next(100, 1000).ToString() + ".zip");
                        _objzip.Save(Response.OutputStream);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Site");
                }

            }
            catch (Exception ex)
            {

            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}