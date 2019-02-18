using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using LinqToExcel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VocabularyController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult IndexAjax(string searchvocabulary = null, int start = 0, int view = 10)
        {

            var listvocabulary = db.vocabularies.ToList();


          

            if (searchvocabulary != null)
            {
                listvocabulary = listvocabulary.Where(x => x.id.ToString().Contains(searchvocabulary) || x.name.ToString().Contains(searchvocabulary) || x.code.ToString().Contains(searchvocabulary) || x.description.ToString().Contains(searchvocabulary)).ToList();
            }
            var count = listvocabulary.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = count;
            ViewBag.ViewOf = count;
            var dbData = listvocabulary.OrderBy(t => t.id).Skip(start).Take(view).ToList();



            return PartialView(dbData);


        }



        // GET: /vocabulary/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vocabulary vocabulary = db.vocabularies.Find(id);
            if (vocabulary == null)
            {
                return HttpNotFound();
            }
            return View(vocabulary);
        }
        public ActionResult _AjaxAutoComplete(string query)
        {
            var list = db.items.Join(db.artgrps, it => it.GROUPNO, grp => grp.GROUPNO, (it, grp) => new AllModel { tblitem = it, tblGroup = grp }).Where(x => x.tblitem.ARTNAME.ToLower().Contains(query.ToLower()) && x.tblitem.EXPORTABLE == "T").ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTNAME, des = x.tblitem.INFO, grp = x.tblGroup.GROUPNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        // GET: /vocabulary/Create
        public ActionResult Create()
        {
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;

            return View(new AllModel {tblVocabulary = new vocabulary()});
        }

        // POST: /vocabulary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(AllModel model)
        {


            try
            {
                if (model.tblVocabularyArray!=null)
                {
                    foreach (var item in model.tblVocabularyArray)
                    {
                        var check = db.vocabularies.FirstOrDefault(x => x.code == item.code);
                        if (check==null)
                        {
                            db.vocabularies.Add(item);
                        }
                        
                    }
                }
              
                
              
                db.SaveChanges();
             


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
            return RedirectToAction("Index");
        }

        // GET: /vocabulary/Edit/5
        public ActionResult Edit(string code)
        {
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vocabulary = db.vocabularies.ToList().Where(x=>x.code.ToLower()== code.ToLower()).ToList();
            return View(new AllModel {listVocabulary = vocabulary });
        }

        // POST: /vocabulary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AllModel model)
        {

            //var tem = db.vocabularies.Find(vocabulary.id);
            if (model.tblVocabularyArray!=null)
            {
                foreach (var item in model.tblVocabularyArray)
                {
                    var tem = db.vocabularies.Find(item.id);
                    tem.name = item.name;
                    tem.description = item.description;
                    tem.language = item.language;
                    tem.code = item.code;

                    db.Entry(tem).State = EntityState.Modified;
                }
            }
           
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: /vocabulary/Delete/5
        public ActionResult Delete(int? id)
        {
            vocabulary vocabulary = db.vocabularies.Find(id);
            db.vocabularies.Remove(vocabulary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /vocabulary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vocabulary vocabulary = db.vocabularies.Find(id);
            db.vocabularies.Remove(vocabulary);
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

