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
    public class PromotionController : Controller
    {
        private veebdbEntities db = new veebdbEntities();

        // GET: /Promotion/
        public ActionResult Index()
        {
            //var list= db.Promotiontypes.Where(x => x.status == 1).ToList();
            //list.Insert(0, new Promotiontype {id = 0, name = "Type"});
            //ViewBag.ListTypePromotion = list;
            return View();
        }
        public ActionResult IndexAjax(int start = 0, int view = 10, int type = 0)
        {
            var listPromotion = db.promotions.Select(x => new AllModel { tblPromotion = x }).ToList();

            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listPromotion.Count;
            listPromotion = listPromotion.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listPromotion.Count;

            return PartialView(listPromotion);
        }
        // GET: /Promotion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promotion Promotion = db.promotions.Find(id);
            if (Promotion == null)
            {
                return HttpNotFound();
            }
            return View(Promotion);
        }

        // GET: /Promotion/Create
        public ActionResult Create()
        {


            return View(new promotion { LASTDATE = DateTime.Now, FIRSTDATE = DateTime.Now });
        }

        // POST: /Promotion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(promotion promotion, HttpPostedFileBase[] files,String[] usertypes, string value)
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



                        }
                    }
                }
                var proId = db.promotions.OrderByDescending(x => x.PROMOTIONNO).FirstOrDefault();
                if (proId != null)
                {
                    promotion.PROMOTIONNO = proId.PROMOTIONNO + 1;
                }
                else
                {
                    promotion.PROMOTIONNO = 1;
                }

                promotion.TYPEUSERS = String.Join(",", usertypes);
                promotion.CREATED = DateTime.Now;
                db.promotions.Add(promotion);
                db.SaveChanges();
                
                if (promotion.QUANTITY != 0)
                {
                    for (int i = 0; i < promotion.QUANTITY; i++)
                    {
                        var tblPro = new promotionrule
                        {
                           PROMOTIONCODE = "KM" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
                           PROMOTIONNO = promotion.PROMOTIONNO,
                            ELIGIBILITYNO = 1,
                            TYPENO = promotion.TYPENO,
                            ADJUSTMENTVALUE = decimal.Parse(promotion.VALUEPROMOTION),
                            ADJUSTMENTTYPENO = 1,
                          CREATED = DateTime.Now,
                          LASTCHANGE = DateTime.Now,
                        PRICEGROUPNO =1
                        };
                        db.promotionrules.Add(tblPro);
                       

                    }
                    db.SaveChanges();
                }
                else
                {
                    /* var tblPro = new promotionrule
                     {
                         PROMOTIONCODE = promotion.PROMOTIONCODE.ToUpper(),
                         PROMOTIONNO = promotion.PROMOTIONNO,
                         ELIGIBILITYNO = 1,
                         TYPENO = promotion.TYPENO,
                         ADJUSTMENTVALUE = decimal.Parse(promotion.VALUEPROMOTION),
                         ADJUSTMENTTYPENO = 1,
                         CREATED = DateTime.Now,
                         LASTCHANGE = DateTime.Now,
                         PRICEGROUPNO = 1
                     };
                     db.promotionrules.Add(tblPro);

                     db.SaveChanges();*/

                }
                return RedirectToAction("Index");
            }

            return View(promotion);
        }

        // GET: /Promotion/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promotion Promotion = db.promotions.Find(id);
            if (Promotion == null)
            {
                return HttpNotFound();
            }
            return View(Promotion);
        }


        [HttpPost]

        public ActionResult Edit(promotion promotion, HttpPostedFileBase[] files, String[] usertypes, string value)
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
                            path = Path.Combine(Server.MapPath("/Content/ProductImage"),
                                Path.GetFileName(files[i].FileName));
                            var picId = db.artlinks.OrderByDescending(x => x.LINENO).FirstOrDefault();
                            files[i].SaveAs(path);



                        }
                    }
                }
                promotion.TYPEUSERS = String.Join(",", usertypes);
                promotion.LASTCHANGE = DateTime.Now;
                db.Entry(promotion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(promotion);
        }

        // GET: /Promotion/Delete/5
        public ActionResult Delete(int? id)
        {
            promotion Promotion = db.promotions.Find(id);
            db.promotions.Remove(Promotion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: /Promotion/Delete/5
        //[HttpPost, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Promotion Promotion = db.promotions.Find(id);
        //    db.promotions.Remove(Promotion);
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
