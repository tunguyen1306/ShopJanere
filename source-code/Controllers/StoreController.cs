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
    public class StoreController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /Store/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10)
        {
            var listStore = db.stores.ToList();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listStore.Count;
            listStore= listStore.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listStore.Count;
          
            return PartialView(listStore);
        }
        // GET: /Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: /Store/Create
        public ActionResult Create()
        {
           
            return View(new AllModel { tblStore = new store() });
        }

        // POST: /Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblStoreArray != null)
            {

                var storeF = new store
                {
                    CodeLanguage = model.tblStoreArray[0].CodeLanguage.ToLower(),
                    Name = model.tblStoreArray[0].Name,
                    Location = model.tblStoreArray[0].Location,
                    Description = model.tblStoreArray[0].Description
                };
                db.stores.Add(storeF);
                db.SaveChanges();
                var updateItem = db.stores.Find(storeF.Id);
                updateItem.IdCurrentItem = storeF.Id;
                db.Entry(updateItem).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in model.tblStoreArray.Skip(1).ToList())
                {

                    var tblStore = new store
                    {
                        CodeLanguage = item.CodeLanguage.ToLower(),
                        IdCurrentItem = storeF.IdCurrentItem,
                        Name = item.Name,
                       Location = item.Location,
                       Description = item.Description,
                       
                };
                  
                    db.stores.Add(tblStore);
                }
              
                if (inputfile != null)
                {
                    for (int i = 0; i < inputfile.Length; i++)
                    {
                        if (inputfile[i] != null)
                        {

                            string path = "";
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile[i].FileName));
                            inputfile[i].SaveAs(path);
                            updateItem.UrlImage = "/Content/ProductImage/" + inputfile[i].FileName;


                        }
                    }
                }  db.SaveChanges();
         
               
            }

          return RedirectToAction("Index");

           
        }

        // GET: /Store/Edit/5
        public ActionResult Edit(int? id)
        {
            var proItem = db.stores.FirstOrDefault(x => x.Id == id);
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;


            foreach (var itemLang in list)
            {
                var pro = db.stores.FirstOrDefault(x => x.IdCurrentItem == id && x.CodeLanguage == itemLang.language.ToLower());
                if (pro == null)
                {

                    if (proItem != null)
                    {


                        var tblItem = new store
                        {
                            CodeLanguage = itemLang.language.ToLower(),
                            Name = proItem.Name,
                            Location = proItem.Location,
                            Description = proItem.Description,
                            UrlImage = proItem.UrlImage,
                            IdCurrentItem = proItem.IdCurrentItem
                        };

                        db.stores.Add(tblItem);

                    }

                }

            }
            db.SaveChanges();
            var item = db.stores.ToList().Where(x => x.IdCurrentItem == id).ToList();
            return View(new AllModel { listStore = item, tblStore = proItem });
        }

        // POST: /Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblStoreArray != null)
            {
                foreach (var item in model.tblStoreArray)
                {
                    var tem = db.stores.Find(item.Id);

                 
                    tem.CodeLanguage = item.CodeLanguage.ToLower();
                    tem.Name = item.Name;
                    tem.Location = item.Location;
                    tem.Description = item.Description;

                    tem.UrlImage = item.UrlImage;
                    db.Entry(tem).State = EntityState.Modified;
                    if (inputfile != null)
                    {
                        for (int i = 0; i < inputfile.Length; i++)
                        {
                            if (inputfile[i] != null)
                            {

                                string path = "";
                                path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile[i].FileName));
                                inputfile[i].SaveAs(path);
                                tem.UrlImage = "/Content/ProductImage/" + inputfile[i].FileName;


                            }
                        }
                    }

                }

            }

            db.SaveChanges();


            return RedirectToAction("Index");
        }

        // GET: /Store/Delete/5
        public ActionResult Delete(int? id)
        {
            store store = db.stores.Find(id);
            db.stores.Remove(store);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /Store/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    store store = db.stores.Find(id);
        //    db.stores.Remove(store);
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
