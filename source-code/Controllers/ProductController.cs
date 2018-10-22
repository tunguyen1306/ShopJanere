using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: /Product/
        public ActionResult Index(FormCollection FormCollection, int? Page_No,int? Page_Size)
        {
            var result = db.items.ToList();
            //result[0].
            var testdata = FormCollection["ddlPosition"];
            int defaSize = 20;
            if (FormCollection["Size_Of_Page"] != null)
            {
                defaSize = int.Parse(FormCollection["Size_Of_Page"]);
            }
            if (Page_Size != null)
            {
                defaSize = Page_Size ?? 20;
            }
            ViewBag.ValueToSet = defaSize;
            int No_Of_Page = (Page_No ?? 1);
            ViewBag.CategoryList = db.categories.ToList();
            ViewBag.WarhouseList = db.categories.ToList();

            return View(result.ToPagedList(No_Of_Page, defaSize));



        }

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
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ARTNO,ARTCODE,ARTNAME,LONGNAME,ARTTYPE,EXPORTABLE,STOCKITEM,PRODUCERNO,CATEGORYNO,COLORNO,INFO,WEBPRICE,SPECIALOFFER,PICTURENAME,NEWUNTIL,GROUPNO,AUTHORIZABLE,RESTRICTED,NOTPOST,NOTADDPOSTAGEFEE,WEIGHT,COLLATIONUNIT,INFO1,INFO2,INFO3,INFO4,INFO5,INFO6,LEN,HEIGHT,WIDTH,UNITNAME,UNITDESCR,CHECKBOX1,CHECKBOX2,CHECKBOX3,CHECKBOX4,CHECKBOX5,CHECKBOX6,VAT,PACKAGEVOLUME,PACKAGEVOLUME2,SIZENO,SIZETBLNO,SUPPLYCYCLENO,DISCGROUP,WEBID,PAYMENTTERM,LASTCHANGE,CREATED")] item item)
        {
            if (ModelState.IsValid)
            {
                db.items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ARTNO,ARTCODE,ARTNAME,WEBPRICE,CATEGORYNO,,PICTURENAME,IsBestSeller")] item item, HttpPostedFileBase inputfile)
        {
            if (ModelState.IsValid)
            {
                //save file
                if (inputfile != null)//have image
                {
                    if (inputfile.ContentLength > 0)
                    {
                        string path = "";
                        try
                        {
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile.FileName));
                            inputfile.SaveAs(path);
                            var tem = db.items.Where(m => m.ARTCODE == item.ARTCODE).FirstOrDefault();
                            tem.ARTNO = item.ARTNO;
                            tem.WEBPRICE = item.WEBPRICE;
                            tem.CATEGORYNO = item.CATEGORYNO;
                            tem.ARTNAME = item.ARTNAME;
                            tem.IsBestSeller = item.IsBestSeller;
                            tem.PICTURENAME = inputfile.FileName;
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
                }
                else
                {
                    var tem = db.items.Where(m => m.ARTCODE == item.ARTCODE).FirstOrDefault();
                    tem.WEBPRICE = item.WEBPRICE;
                    tem.CATEGORYNO = item.CATEGORYNO;
                    tem.ARTNO = item.ARTNO;
                    tem.ARTNAME = item.ARTNAME;
                    tem.IsBestSeller = item.IsBestSeller;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                
            }
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
    }
}
