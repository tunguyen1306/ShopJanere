using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class AdminController : Controller
    {
        private veebdbEntities db = new veebdbEntities();
        public ActionResult Index(String tab = "all")
        {
            ViewBag.GroupList = db.artgrps.ToList();
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();
            ViewBag.Tab = tab;
            return View();
        }
        public ActionResult IndexAjax(string keywork=null,string tab = "all", int start = 0, int view = 10, int groupId = 0, int storeId = 0, int stockId = 0, int warehouseId = 0)
        {

            var listProduct = db.items.Where(x => x.ARTNO > 0 && x.CodeLanguage=="english");
            if (keywork!=null)
            {
                listProduct = listProduct.Where(x => x.ARTNO.ToString().Contains(keywork) || x.ARTNAME.ToString().Contains(keywork) || x.ARTCODE.ToString().Contains(keywork) || x.INFO.ToString().Contains(keywork) || x.LEN.ToString().Contains(keywork) || x.WEIGHT.ToString().Contains(keywork) || x.HEIGHT.ToString().Contains(keywork));
            }
            if (groupId != 0)
            {
                listProduct = listProduct.Where(x => x.GROUPNO == groupId);
            }
            if (storeId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == storeId);
            }
            if (stockId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == stockId);
            }
            if (warehouseId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == warehouseId);
            }


          


            var _count = listProduct.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = _count;
            ViewBag.ViewOf = _count;
            var db_data = listProduct.OrderBy(t => t.ARTNO).Skip(start).Take(view).ToList();
            foreach (var item in db_data)
            {
                var tblPic = db.artlinks.FirstOrDefault(x => x.ARTNO == item.ARTNO);
                item.PICTURENAME = tblPic != null ? tblPic.LINK : "";

            }



            var datas = db_data.Select(t => new AllModel {  tblitem = t }).ToList();
            return PartialView(datas);


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
            return View(new AllModel { tblitem = new item() });
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
        [ValidateInput(false)]
        public ActionResult Create(AllModel model, HttpPostedFileBase[] inputfile)
        {
            try
            {
                if (model.tblProductArray!=null)
                {
                    var artCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper() ;
                    var itemF= new item();
                    itemF.ARTTYPE = 1;

                    itemF.CREATED = DateTime.Now;
                    itemF.LASTCHANGE = DateTime.Now;
                    itemF.ARTCODE = artCode;
                    itemF.ARTNAME = model.tblProductArray[0].ARTNAME;
                    itemF.INFO = model.tblProductArray[0].INFO;
                    itemF.CodeLanguage = model.tblProductArray[0].CodeLanguage.ToLower();
                    itemF.GROUPNO = model.tblitem.GROUPNO;
                    itemF.IsBestSeller = model.tblitem.IsBestSeller;
                    itemF.EXPORTABLE = model.tblitem.EXPORTABLE;
                    itemF.SPECIALOFFER = model.tblitem.SPECIALOFFER;
                    itemF.STOCKITEM = model.tblitem.STOCKITEM;
                    itemF.AUTHORIZABLE = model.tblitem.AUTHORIZABLE;
                    itemF.RESTRICTED = model.tblitem.RESTRICTED;
                    itemF.NOTPOST = model.tblitem.NOTPOST;
                    itemF.NOTADDPOSTAGEFEE = model.tblitem.NOTADDPOSTAGEFEE;
                    itemF.WIDTH = model.tblitem.WIDTH;
                    itemF.WEIGHT = model.tblitem.WEIGHT;
                    itemF.LEN = model.tblitem.LEN;
                    itemF.HEIGHT = model.tblitem.HEIGHT;
                    itemF.WEBPRICE = model.tblitem.WEBPRICE;
                    db.items.Add(itemF);
                    db.SaveChanges();
                    var updateItem = db.items.Find(itemF.ARTNO);
                    updateItem.IdCurrentItem = itemF.ARTNO;
                    db.Entry(updateItem).State=EntityState.Modified;
                    db.SaveChanges();
                    foreach (var item in model.tblProductArray.Skip(1).ToList())
                    {
                        
                        var tblItem = new item();
                        tblItem.ARTTYPE = 1;
                      
                        tblItem.CREATED = DateTime.Now;
                        tblItem.LASTCHANGE = DateTime.Now;
                        tblItem.ARTCODE = itemF.ARTCODE;
                        tblItem.ARTNAME = item.ARTNAME;
                        tblItem.INFO = item.INFO;
                        tblItem.CodeLanguage = item.CodeLanguage.ToLower();
                        tblItem.GROUPNO = model.tblitem.GROUPNO;
                        tblItem.IsBestSeller = model.tblitem.IsBestSeller;
                        tblItem.EXPORTABLE = model.tblitem.EXPORTABLE;
                        tblItem.SPECIALOFFER = model.tblitem.SPECIALOFFER;
                        tblItem.STOCKITEM = model.tblitem.STOCKITEM;
                        tblItem.AUTHORIZABLE = model.tblitem.AUTHORIZABLE;
                        tblItem.RESTRICTED = model.tblitem.RESTRICTED;
                        tblItem.NOTPOST = model.tblitem.NOTPOST;
                        tblItem.NOTADDPOSTAGEFEE = model.tblitem.NOTADDPOSTAGEFEE;
                        tblItem.WIDTH = model.tblitem.WIDTH;
                        tblItem.WEIGHT = model.tblitem.WEIGHT;
                        tblItem.LEN = model.tblitem.LEN;
                        tblItem.HEIGHT = model.tblitem.HEIGHT;
                        tblItem.IdCurrentItem = itemF.IdCurrentItem;
                        tblItem.WEBPRICE = model.tblitem.WEBPRICE;
                        db.items.Add(tblItem);
                    }
                    db.SaveChanges();
                    if (inputfile!=null)
                    {
                        for (int i = 0; i < inputfile.Length; i++)
                        {
                            if (inputfile[i] != null)
                            {

                                string path = "";
                                path = Path.Combine(Server.MapPath("/Content/ProductImage"), Path.GetFileName(inputfile[i].FileName));

                                inputfile[i].SaveAs(path);
                                var i1 = itemF.IdCurrentItem > 0 ? itemF.IdCurrentItem:0;
                                if (i1 != null)
                                {
                                    var tblPic = new artlink
                                    {
                                        ARTNO =(int)i1,
                                        LASTCHANGE = DateTime.Now,
                                        CREATED = DateTime.Now,
                                        LINK = inputfile[i].FileName

                                    };
                                    db.artlinks.Add(tblPic);
                                }
                                db.SaveChanges();
                            }
                        }
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
        public ActionResult Edit(int id)
        {
            var proItem = db.items.FirstOrDefault(x => x.ARTNO == id);
            var category = db.categories.ToList();
            category.Insert(0, new category { CATEGORYNO = 0, CATEGORYNAME = "Select Category" });
            ViewBag.CategoryList = category;
            ViewBag.WarhouseList = db.warehouses.ToList();
            ViewBag.stockList = db.stockcods.ToList();
            ViewBag.StoreList = db.stores.ToList();
            var metagroup = db.metagrups.ToList();
            metagroup.Insert(0, new metagrup { METAGROUPNO = 0, METAGROUPNAME = "Select Meta Group" });
            ViewBag.MetaGroupList = metagroup;
          
            var list = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            ViewBag.ListCountry = list;
            if (id ==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var itemLang in list)
            {
                var pro = db.items.FirstOrDefault(x => x.IdCurrentItem == id && x.CodeLanguage == itemLang.language.ToLower());
                if (pro==null)
                {
                  
                    if (proItem!=null)
                    {
                       

                        var tblItem = new item();
                        tblItem.ARTTYPE = 1;

                        tblItem.CREATED = DateTime.Now;
                        tblItem.LASTCHANGE = DateTime.Now;
                        tblItem.ARTCODE = proItem.ARTCODE;
                        tblItem.ARTNAME = proItem.ARTNAME;
                        tblItem.INFO = proItem.INFO;
                        tblItem.CodeLanguage = itemLang.language.ToLower();
                        tblItem.GROUPNO = proItem.GROUPNO;
                        tblItem.IsBestSeller = proItem.IsBestSeller;
                        tblItem.EXPORTABLE = proItem.EXPORTABLE;
                        tblItem.SPECIALOFFER = proItem.SPECIALOFFER;
                        tblItem.STOCKITEM = proItem.STOCKITEM;
                        tblItem.AUTHORIZABLE = proItem.AUTHORIZABLE;
                        tblItem.RESTRICTED = proItem.RESTRICTED;
                        tblItem.NOTPOST = proItem.NOTPOST;
                        tblItem.NOTADDPOSTAGEFEE = proItem.NOTADDPOSTAGEFEE;
                        tblItem.WIDTH = proItem.WIDTH;
                        tblItem.WEIGHT = proItem.WEIGHT;
                        tblItem.LEN = proItem.LEN;
                        tblItem.HEIGHT = proItem.HEIGHT;
                        tblItem.IdCurrentItem = proItem.IdCurrentItem;
                        db.items.Add(tblItem);
                        var tblLink = db.artlinks.FirstOrDefault(x => x.ARTNO == proItem.ARTNO);
                        if (tblLink!=null)
                        {
                            var tblPic = new artlink
                            {

                                ARTNO = tblItem.ARTNO,
                                LASTCHANGE = DateTime.Now,
                                CREATED = DateTime.Now,
                                LINK = tblLink.LINK

                            };
                            db.artlinks.Add(tblPic);
                        }
                       
                    }
                 
                }
               
            }
            db.SaveChanges();
            var item = db.items.ToList().Where(x=>x.IdCurrentItem==id).ToList();
            return View(new AllModel { listProduct = item,tblitem = proItem });
    
        }
        public ActionResult GetMetaGroup(int? id)
        {

            item item = db.items.Find(id);
            if (item != null && item.GROUPNO != null)
            {
                var groupNo = db.artgrps.FirstOrDefault(x => x.GROUPNO == item.GROUPNO && x.CodeLanguage=="english");
                if (groupNo != null && groupNo.METAGROUPNO != null)
                {
                    var metagroupNo = db.metagrups.FirstOrDefault(x => x.METAGROUPNO == groupNo.METAGROUPNO);
                    return Json(metagroupNo, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(AllModel model, HttpPostedFileBase[] inputfile)
        {
            if (model.tblProductArray!=null)
            {
                foreach (var item in model.tblProductArray)
                {
                    var tem = db.items.Find(item.ARTNO);
                    tem.WEBPRICE = model.tblitem.WEBPRICE;
                    tem.CATEGORYNO = model.tblitem.CATEGORYNO;
                    tem.ARTNAME = item.ARTNAME;
                    tem.GROUPNO = model.tblitem.GROUPNO;
                    tem.INFO = item.INFO;
                    tem.IsBestSeller = model.tblitem.IsBestSeller;
                    tem.WEBPRICE =item.WEBPRICE;
                    tem.EXPORTABLE = model.tblitem.EXPORTABLE;
                    tem.STOCKITEM = model.tblitem.STOCKITEM;
                    tem.SPECIALOFFER = model.tblitem.SPECIALOFFER;
                    tem.AUTHORIZABLE = model.tblitem.AUTHORIZABLE;
                    tem.RESTRICTED = model.tblitem.RESTRICTED;
                    tem.NOTPOST = model.tblitem.NOTPOST;
                    tem.NOTADDPOSTAGEFEE = model.tblitem.NOTADDPOSTAGEFEE;
                    tem.WIDTH = model.tblitem.WIDTH;
                    tem.WEIGHT = model.tblitem.WEIGHT;
                    tem.HEIGHT = model.tblitem.HEIGHT;
                    tem.LEN = model.tblitem.LEN;
                 

                    db.Entry(tem).State = EntityState.Modified;
                    if (inputfile!=null)
                    {
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
                                    LINENO = picId.LINENO + 1,
                                    ARTNO = item.ARTNO,
                                    LASTCHANGE = DateTime.Now,
                                    CREATED = DateTime.Now,
                                    LINK = inputfile[i].FileName

                                };
                                db.artlinks.Add(tblPic);

                            }
                        }
                    }
                   
                }
                
            }
          
            db.SaveChanges();
          

            return RedirectToAction("Index");
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
        public ActionResult ImportExcel(HttpPostedFileBase fileUpload)
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
                                var check = db.items.ToList().FirstOrDefault(x => x.ARTCODE.ToLower() == a.ARTCODE.ToLower());
                                if (check == null)
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
                        ViewBag.FileError = "Code duplicate " + error;
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
        public ActionResult DeleteImg(int? id)
        {

            var item = db.artlinks.Find(id);
            if (item != null)
            {
                db.artlinks.Remove(item);
                db.SaveChanges();
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
                var _items = db.items.Where(x => _ids.Contains(x.ARTNO));
                foreach (var item in _items)
                {
                    // update
                }
                _status = true;
            }
            catch (Exception)
            {

                msg = "Update Failed";
            }
            return Json(new { status = _status, mgs = msg });
        }
        public ActionResult ExportExcel(string[] ids=null,int groupId = 0, int storeId = 0, int stockId = 0, int warehouseId = 0)
        {
            var listProduct = db.items.Where(x => x.ARTNO > 0);
            if (groupId != 0)
            {
                listProduct = listProduct.Where(x => x.GROUPNO == groupId);
            }
            if (storeId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == storeId);
            }
            if (stockId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == stockId);
            }
            if (warehouseId != 0)
            {
                listProduct = listProduct.Where(x => x.CATEGORYNO == warehouseId);
            }


            var listAll = (from pro in listProduct
                           join cate in db.artgrps on pro.GROUPNO equals cate.GROUPNO
                           select new { tblitem = pro, tblGroup = cate });


            var _count = listAll.Count();
           
            ViewBag.Total = _count;
            ViewBag.ViewOf = _count;
            var db_data = listAll.OrderBy(t => t.tblitem.ARTNO).ToList();
            foreach (var item in db_data)
            {
                var tblPic = db.artlinks.FirstOrDefault(x => x.ARTNO == item.tblitem.ARTNO);
                item.tblitem.PICTURENAME = tblPic != null ? tblPic.LINK : "";

            }


            var _ids = ids.Select(t => int.Parse(t)).ToList();
           
            var datas = db_data.Where(x => _ids.Contains(x.tblitem.ARTNO)).Select(t => t.tblitem).ToList();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Contact.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(datas, Response.Output);
            Response.End();
            return View();
        }
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }


    }
}

