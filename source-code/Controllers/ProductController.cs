using System;
using System.Collections.Generic;
using System.Data;
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
namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        public ActionResult Index()
        {
            ViewBag.CategoryList = db.categories.ToList();
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();

            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10, int catId = 0, int storeId = 0, int stockId = 0, int ware = 0)
        {

            var listProduct = db.items.ToList();
            if (catId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == catId).ToList();
            }
            if (storeId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == storeId).ToList();
            }
            if (stockId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == stockId).ToList();
            }
            if (ware != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == ware).ToList();
            }
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listProduct.Count;
            listProduct = listProduct.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listProduct.Count;
            ViewBag.CategoryList = db.categories.ToList();
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();

            return PartialView(listProduct);


        }
        // GET: /Product/
        //public ActionResult Index(FormCollection FormCollection, int? Page_No,int? Page_Size)
        //{
        //    var result = db.items.ToList();
        //    //result[0].
        //    var testdata = FormCollection["ddlPosition"];
        //    int defaSize = 20;
        //    if (FormCollection["Size_Of_Page"] != null)
        //    {
        //        defaSize = int.Parse(FormCollection["Size_Of_Page"]);
        //    }
        //    if (Page_Size != null)
        //    {
        //        defaSize = Page_Size ?? 20;
        //    }
        //    ViewBag.ValueToSet = defaSize;
        //    int No_Of_Page = (Page_No ?? 1);
        //    ViewBag.CategoryList = db.categories.ToList();
        //    ViewBag.WarhouseList = db.warehouses.ToList();
        //    ViewBag.stockList = db.stockcods.ToList();
        //    ViewBag.StoreList = db.stores.ToList();

        //    return View(result.ToPagedList(No_Of_Page, defaSize));



        //}

        // GET: /Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            var category = db.categories.ToList();
            category.Insert(0, new category { CATEGORYNO = 0, CATEGORYNAME = "Select Category" });
            ViewBag.CategoryList = category;
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();
            var metagroup = db.metagrups.ToList();
            metagroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.MetaGroupList = metagroup;
            return View(new item());
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(item item, HttpPostedFileBase[] inputfile)
        {
            var category = db.categories.ToList();
            category.Insert(0, new category { CATEGORYNO = 0, CATEGORYNAME = "Select Category" });
            ViewBag.CategoryList = category;
           
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();
            var metagroup = db.metagrups.ToList();
            metagroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.MetaGroupList = metagroup;
            try
            {

                var proId = db.items.OrderByDescending(x => x.ARTNO).FirstOrDefault();
                item.ARTTYPE = 1;
                item.EXPORTABLE = "";
                item.ARTCODE = Guid.NewGuid().ToString().Substring(0,6).ToUpper();
                item.CREATED = DateTime.Now;
                item.LASTCHANGE = DateTime.Now;
                item.ARTNO = proId.ARTNO + 1;
                db.items.Add(item);
                db.SaveChanges();
                for (int i = 0; i < inputfile.Length; i++)
                {
                    if (inputfile[i] != null)
                    {

                        string path = "";
                        path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile[i].FileName));
                        var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                        inputfile[i].SaveAs(path);
                        var tblPic = new artlink
                        {
                            LINENO = picId.LINENO+1,
                            ARTNO = item.ARTNO,
                            LASTCHANGE = DateTime.Now,
                            CREATED = DateTime.Now,
                            LINK = path

                        };
                        db.artlinks.Add(tblPic);
                        db.SaveChanges();
                    }
                }
               
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

        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
        {
            var category = db.categories.ToList();
            category.Insert(0, new category { CATEGORYNO = 0, CATEGORYNAME = "Select Category" });
            ViewBag.CategoryList = category;
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();
            var metagroup = db.metagrups.ToList();
            metagroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.MetaGroupList = metagroup;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(item item, HttpPostedFileBase[] inputfile)
        {

            var tem = db.items.FirstOrDefault(m => m.ARTCODE == item.ARTCODE);
            tem.WEBPRICE = item.WEBPRICE;
            tem.CATEGORYNO = item.CATEGORYNO;
            tem.ARTNO = item.ARTNO;
            tem.ARTNAME = item.ARTNAME;
            tem.IsBestSeller = item.IsBestSeller;

            for (int i = 0; i < inputfile.Length; i++)
            {
                if (inputfile[i] != null)
                {

                    string path = "";
                    path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile[i].FileName));
                    inputfile[i].SaveAs(path);
                    var tblPic = new artlink
                    {
                        ARTNO = tem.ARTNO,
                        LASTCHANGE = DateTime.Now,
                        CREATED = DateTime.Now,
                        LINK = path

                    };
                    db.artlinks.Add(tblPic);

                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");


            return View(item);
        }
        // GET: /Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item item = db.items.Find(id);
            db.items.Remove(item);
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

        public ActionResult _pGetGroupByIdMeta(int id = 0)
        {
            var groupList = new List<artgrp>();
            if (id != 0)
            {
                groupList = db.artgrps.ToList().Where(x => x.METAGROUPNO == id).ToList();
            }





            return PartialView(groupList);
        }
    }
}
