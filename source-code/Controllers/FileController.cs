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
            return View(new file());
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public ActionResult Create(file file, HttpPostedFileBase inputfile)
        {
            //  if (ModelState.IsValid)
            // {
            try
            {
                //save file
                if (file != null && inputfile.ContentLength > 0)
                {
                    string path = "";
                    try
                    {
                        path = Path.Combine(Server.MapPath("/UploadFile"), Path.GetFileName(inputfile.FileName));

                        inputfile.SaveAs(path);
                        //save other infomation
                        file.FileName = inputfile.FileName;
                        file.Status = "Active";
                        file.FileAddress = path;
                        file.UploadDate = DateTime.Now;
                        file.UploadBy = User.Identity.Name;
                        file.Kind = inputfile.ContentType;
                        db.files.Add(file);
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
        public ActionResult Edit(int? id)
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

        // POST: /Containt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Edit( file file, HttpPostedFileBase inputfile)
        {
            try
            {
                var ftblile = db.files.Find(file.Id);
                ftblile.Title = file.Title;
                ftblile.FileName = file.FileName;
                ftblile.FileAddress = file.FileAddress;
                ftblile.Status = file.Status;
                ftblile.Kind = file.Kind;
                ftblile.Description = file.Description;
                //save file
                if ( inputfile!=null)
                {
                    string path = "";
                    try
                    {
                        path = Path.Combine(Server.MapPath("/UploadFile"), Path.GetFileName(inputfile.FileName));

                        inputfile.SaveAs(path);
                        //save other infomation
                        ftblile.FileName = inputfile.FileName;
                        ftblile.FileAddress = path;
                        ftblile.UploadDate = DateTime.Now;
                        ftblile.UploadBy = User.Identity.Name;
                        ftblile.Kind = inputfile.ContentType;
                        db.Entry(ftblile).State = EntityState.Modified;
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
                   
                    db.Entry(ftblile).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            catch (Exception ex)
            {

                
            }
               
            return View(file);
          
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