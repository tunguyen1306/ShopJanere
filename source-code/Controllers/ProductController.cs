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

            var listProduct = db.items.Where(x=>x.ARTNO>0).ToList();
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

            //var id = 1;
            //var query = database.Posts    // your starting point - table in the "from" statement
            //   .Join(database.Post_Metas, // the source table of the inner join
            //      post => post.ID,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
            //      meta => meta.Post_ID,   // Select the foreign key (the second part of the "on" clause)
            //      (post, meta) => new { Post = post, Meta = meta }) // selection
            //   .Where(postAndMeta => postAndMeta.Post.ID == id);
            var listAll = (from pro in listProduct.AsEnumerable()
                join cat in db.categories on pro.CATEGORYNO equals cat.CATEGORYNO
                select new AllModel {tblitem = pro ,tblCategory = cat} ).ToList();
               

            foreach (var item in listAll)
            {
                var tblPic = db.artlinks.FirstOrDefault(x => x.ARTNO == item.tblitem.ARTNO);
                item.tblitem.PICTURENAME = tblPic != null ? tblPic.LINK : "";
               
            }
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = listAll.Count;
            listAll = listAll.Skip(start).Take(view).ToList();
            ViewBag.ViewOf = listAll.Count;
            ViewBag.CategoryList = db.categories.ToList();
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();

            return PartialView(listAll);


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
        //public ActionResult ImnportData(HttpPostedFileBase inputfile)
        //{
        //    string ExcelContentType = "application/vnd.ms-excel";
        //    string Excel2010ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    if (inputfile!=null)
        //    {
        //        //Check the Content Type of the file 
               
        //            try
        //            {
        //            string path = string.Concat(Server.MapPath("~/Excel/" + inputfile.FileName));
        //            inputfile.SaveAs(path);
        //            string excelConnectionString = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = { 0 }; Extended Properties = Excel 8.0", path);
        //            OleDbConnection connection = new OleDbConnection();
        //            connection.ConnectionString = excelConnectionString;
        //            OleDbCommand command = new OleDbCommand("select * from[Sheet1$]" connection);
        //            connection.Open();
        //            DbDataReader dr = command.ExecuteReader();
        //            string sqlConnectionString = @"Data Source =.; Initial Catalog = Wordpress; Integrated Security = True";
        //            SqlBulkCopy bulkInsert = new SqlBulkCopy(sqlConnectionString);
        //            bulkInsert.DestinationTableName = "tbl_bulkupload";
        //            bulkInsert.WriteToServer(dr);
        //            connection.Close();
        //        }

        //            catch (Exception ex)
        //            {
        //                //Label1.Text = ex.Message;
        //            }
                
        //    }
        //    return View();
        //}

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
                item.ARTCODE = string.IsNullOrEmpty(item.ARTCODE)? Guid.NewGuid().ToString().Substring(0,6).ToUpper(): item.ARTCODE;
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
        public FileResult DownloadExcel()
        {
            string path = "/UploadFile/Users.xlsx";
            return File(path, "application/vnd.ms-excel", "Users.xlsx");
        }
        public ActionResult ImportExcel()
        {
            ViewBag.FileError = "";
            return View();
        }
        [HttpPost]
        public ActionResult ImportExcel( HttpPostedFileBase fileUpload)
        {

            List<string> data = new List<string>();
            try
            {
                if (fileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (fileUpload.ContentType == "application/vnd.ms-excel" || fileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {


                        string filename = fileUpload.FileName;
                        string targetpath = Server.MapPath("~/UploadFile/");
                        fileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                        }

                        var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                        var ds = new DataSet();

                        adapter.Fill(ds, "ExcelTable");

                        DataTable dtable = ds.Tables["ExcelTable"];

                        string sheetName = "Sheet1";

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var artistAlbums = from a in excelFile.Worksheet<item>(sheetName) select a;
                        var error = "";
                        var code = 0;
                        foreach (var a in artistAlbums)
                        {
                            try
                            {
                               var check=db.items.ToList().FirstOrDefault(x=>x.ARTCODE.ToLower()==a.ARTCODE.ToLower());
                                if (check==null)
                                {
                                    var proId = db.items.OrderByDescending(x => x.ARTNO).FirstOrDefault();
                                    var tblIttem = new item();
                                    tblIttem.ARTNAME = a.ARTNAME;
                                    tblIttem.SPECIALOFFER = a.SPECIALOFFER;
                                    tblIttem.AUTHORIZABLE = a.AUTHORIZABLE;
                                    tblIttem.RESTRICTED = a.RESTRICTED;
                                    tblIttem.NOTPOST = a.NOTPOST;
                                    tblIttem.NOTADDPOSTAGEFEE = a.NOTADDPOSTAGEFEE;
                                    tblIttem.ARTTYPE = a.ARTTYPE;
                                    tblIttem.EXPORTABLE = a.EXPORTABLE;
                                    tblIttem.STOCKITEM = a.STOCKITEM;
                                    tblIttem.WEIGHT = a.WEIGHT;
                                    tblIttem.LEN = a.LEN;
                                    tblIttem.HEIGHT = a.HEIGHT;
                                    tblIttem.WIDTH = a.WIDTH;
                                    tblIttem.ARTCODE = string.IsNullOrEmpty(a.ARTCODE) ? Guid.NewGuid().ToString().Substring(0, 6).ToUpper() : a.ARTCODE;
                                    tblIttem.CREATED = DateTime.Now;
                                    tblIttem.LASTCHANGE = DateTime.Now;
                                    tblIttem.ARTNO = proId.ARTNO + 1;

                                    db.items.Add(tblIttem);

                                    db.SaveChanges();
                                    
                                }
                                else
                                {
                                    error += a.ARTCODE + ";";
                                }
                                




                            }

                            catch (DbEntityValidationException ex)
                            {
                                ViewBag.FileError = ex.Message;
                            }
                        }
                        ViewBag.FileError = "Code duplicate "+ error;
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return View();
                    }
                    else
                    {

                        return View();
                    }
                }
            else
            {

                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.FileError = ex;
            }
            return View();

        }
    }
}

