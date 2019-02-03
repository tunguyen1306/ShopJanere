using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Helper;
using PayPal;
using PayPal.Api;
using UrlHelper = WebApplication1.Helper.UrlHelper;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
//using MySql.Data.MySqlClient;
using System.Threading;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        veebdbEntities data = new veebdbEntities();
        public ActionResult Index()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "home");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }

            if (Session["LoggedAccount"] == null)
            {
                IntForGuest();
            }

            //List<item> relatedProducts = data.items.Take(100).ToList();
            //List<item> BestSellerProducts = data.items.Where(m=>m.IsBestSeller==true).Take(4).ToList();
            //Session["relatedProducts"] = relatedProducts;
            //Session["BestSellerProducts"] = BestSellerProducts;
            return View();
        }
        public ActionResult BestSeller(int index)
        {
            var check = data.items.Where(m => m.IsBestSeller == true);
            if (check.Count() > 0)
            {
                List<item> BestSellerProducts = check.ToList();
                List<item> showItems = new List<item>();
                int max = BestSellerProducts.Count();

                if (max >= index * 4)
                {
                    int startPostionInList = (index - 1) * 4;
                    int endPostionInList = ((index - 1) * 4 + 4);
                    for (int i = startPostionInList; i < endPostionInList; i++)
                    {
                        item addToList = new item();
                        addToList.PICTURENAME = BestSellerProducts[i].PICTURENAME;
                        addToList.ARTCODE = string.IsNullOrEmpty(BestSellerProducts[i].ARTCODE) ? "" : BestSellerProducts[i].ARTCODE;
                        addToList.ARTNAME = string.IsNullOrEmpty(BestSellerProducts[i].ARTNAME) ? "" : BestSellerProducts[i].ARTNAME;
                        addToList.WEBPRICE = BestSellerProducts[i] == null ? 0 : BestSellerProducts[i].WEBPRICE;
                        addToList.WIDTH = BestSellerProducts[i].WIDTH;
                        addToList.HEIGHT = BestSellerProducts[i].HEIGHT;

                        showItems.Add(addToList);
                    }
                }
                else
                {
                    int startPostionInList = (index - 1) * 4;
                    int endPostionInList = max;
                    for (int i = startPostionInList; i < endPostionInList; i++)
                    {
                        item addToList = new item();
                        addToList.PICTURENAME = BestSellerProducts[i].PICTURENAME;
                        addToList.ARTCODE = string.IsNullOrEmpty(BestSellerProducts[i].ARTCODE) ? "" : BestSellerProducts[i].ARTCODE;
                        addToList.ARTNAME = string.IsNullOrEmpty(BestSellerProducts[i].ARTNAME) ? "" : BestSellerProducts[i].ARTNAME;
                        addToList.WEBPRICE = BestSellerProducts[i] == null ? 0 : BestSellerProducts[i].WEBPRICE;
                        addToList.WIDTH = BestSellerProducts[i].WIDTH;
                        addToList.HEIGHT = BestSellerProducts[i].HEIGHT;

                        showItems.Add(addToList);
                    }
                }
                Session["BestSellerProducts"] = showItems;// BestSellerProducts;
            }
            else
            {
                Session["BestSellerProducts"] = null;
            }



            return View();
        }
        public ActionResult MasterMetaGroup(int index)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var check = data.metagrups.Where(m => m.PARENTNO == 0 && m.IsActive == true && m.CodeLanguage== lang.ToLower());

            if (check.Count() > 0)
            {
                List<metagrup> MasterMetaList = check.ToList();
                List<metagrup> showItems = new List<metagrup>();
                int max = MasterMetaList.Count();

                if (max >= index * 5)
                {
                    int startPostionInList = (index - 1) * 5;
                    int endPostionInList = ((index - 1) * 5 + 5);
                    for (int i = startPostionInList; i < endPostionInList; i++)
                    {
                        metagrup addToList = new metagrup();
                        addToList.METAGROUPNAME = MasterMetaList[i].METAGROUPNAME;
                        addToList.METAGROUPCODE = MasterMetaList[i].METAGROUPCODE;
                        addToList.METAGROUPNO = MasterMetaList[i].METAGROUPNO;
                        showItems.Add(addToList);
                    }
                }
                else
                {
                    int startPostionInList = (index - 1) * 4;
                    int endPostionInList = max;
                    for (int i = startPostionInList; i < endPostionInList; i++)
                    {
                        metagrup addToList = new metagrup();
                        addToList.METAGROUPNAME = MasterMetaList[i].METAGROUPNAME;
                        addToList.METAGROUPCODE = MasterMetaList[i].METAGROUPCODE;
                        addToList.METAGROUPNO = MasterMetaList[i].METAGROUPNO;
                        showItems.Add(addToList);
                    }
                }
                Session["MasterMetaList"] = showItems;// BestSellerProducts;
            }
            else
            {
                Session["MasterMetaList"] = null;
            }
            return View();
        }
        public void IntForGuest()
        {
            var tem = data.users.Where(m => m.Id == 7).FirstOrDefault();
            if (tem != null)
            {
                Session["LoggedAccount"] = null;
                AllLoggedUserInfo userFullInfo = new AllLoggedUserInfo(tem);
                Session["LoggedAccount"] = userFullInfo;
            }
        }
        public ActionResult Product(string Code = "")
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var result = data.items.Where(m => m.ARTCODE == Code && m.CodeLanguage==lang.ToLower()).FirstOrDefault();
            if (result != null)
            {
                var _stock = data.stocks.Where(t => t.ARTNO == result.ARTNO).ToList();
                ViewBag.Stock = _stock;
                var listStocID = _stock.Select(t => t.STOCKNO).Distinct().ToList();
                ViewBag.StockName = data.stockcods.Where(t => listStocID.Contains(t.STOCKNO)).ToList();
                ViewBag.MetaTitle = result.ARTNAME;
                ViewBag.MetaDescription = result.ARTNAME;
                ViewBag.Link = "http://shop.janere.ee/Home/Product?Code=" + result.ARTNO;
                ViewBag.Keyword = result.ARTNAME;

                //var seo = data.seos.FirstOrDefault(x => x.page == "product");
                //if (seo != null)
                //{
                //    ViewBag.MetaTitle = seo.title;
                //    ViewBag.MetaDescription = seo.description;
                //    ViewBag.Link = seo.link;
                //    ViewBag.Keyword = seo.keyword;
                //}
                if (string.IsNullOrEmpty(result.WEBPRICE.ToString()))
                {
                    result.WEBPRICE = 0;
                }
                List<item> relatedProducts = data.items.Where(m => m.CATEGORYNO == result.CATEGORYNO).Take(100).ToList();
                Random r = new Random();
                Session["relatedProducts"] = relatedProducts;
                int cat = result.CATEGORYNO ?? 0;
                ViewBag.categoryId = cat;
                var temCat = data.categories.Where(m => m.CATEGORYNO == cat).FirstOrDefault();
                if (temCat != null)
                {
                    ViewBag.categoryName = data.categories.Where(m => m.CATEGORYNO == cat).FirstOrDefault().CATEGORYNAME;
                }
                else
                {
                    ViewBag.categoryName = "unlocated catagory";
                }
            }
            else
            {
                result=new item();
            }
            return View(result);
        }
        public ActionResult BulkProducts(SearchModel search, int? Page_No, int? Page_Size, bool? pagination)
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "bulkproducts");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            if (pagination.HasValue && pagination.Value)
            {
                search = Session["SearchModel"] == null ? new SearchModel() : (SearchModel)Session["SearchModel"];
            }


            int defaSize = 20;


            if (Page_Size.HasValue)
            {
                defaSize = Page_Size ?? 20;
            }
            ViewBag.ValueToSet = defaSize;

            int No_Of_Page = (Page_No ?? 1);
            var _temp0 = search.ddlGetGroupno;
            var temp0 = search.ddlGetCategory;
            var temp1 = search.ddlPosition;
            var temp2 = search.AllKey == null ? "" : search.AllKey;
            var temp3 = search.ddlGetGroup == null ? "" : search.ddlGetGroup;// FormCollection["ddlGetGroup"];
            var temp4 = search.ddlGetMetaGroup == null ? "" : search.ddlGetMetaGroup;// FormCollection["ddlGetMetaGroup"];
            var temp5 = search.ddlGetItem == null ? "" : search.ddlGetItem;//FormCollection["ddlGetItem"];
            var temp6 = search.ddlGetDINCode == null ? "" : search.ddlGetDINCode;//FormCollection["ddlGetDINCode"];

            Session["SearchModel"] = search;
            var temp8 = search.ddlLength;//int.Parse(FormCollection["ddlLength"]);
            var temp9 = search.ddlGetInterFace;// int.Parse(FormCollection["ddlGetInterFace"]);
            var temp10 = search.ddlGetMaterial;// int.Parse(FormCollection["ddlGetMaterial"]);            
            var temp7 = search.ddlGetDimension;//int.Parse(FormCollection["ddlGetDimension"]);


            var query = from a in data.items
                        join b in data.artgrps on a.GROUPNO equals b.GROUPNO
                        join c in data.metagrups on b.METAGROUPNO equals c.METAGROUPNO
                        where (temp3 == "" || b.GROUPNAME.ToLower().Contains(temp3.ToLower()))
                        && (temp4 == "" || c.METAGROUPNAME.ToLower().Contains(temp4.ToLower()))
                        && (temp6 == "" || a.ARTCODE.ToLower().Contains(temp6.ToLower()))
                        && (temp2 == "" || a.ARTCODE.ToLower().Contains(temp2.ToLower()) || a.ARTNAME.ToLower().Contains(temp2.ToLower()))
                        && (!temp8.HasValue || a.LEN == temp8)
                        && (!temp0.HasValue || a.CATEGORYNO == temp0)
                        && (!_temp0.HasValue || a.GROUPNO == _temp0)
                        select a;



            if (temp1 == 1)
            {
                query = query.OrderByDescending(m => m.ARTNAME);
            }
            else
            {
                query = query.OrderBy(m => m.ARTNAME);
            }



            return View(query.ToPagedList(No_Of_Page, defaSize));

        }
        public ActionResult Login(string message = "", string returnUrl = "")
        {
            Session.Clear();
            Session.Abandon();
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "login");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.Url.ToString());
            ViewBag.ReturnURL = returnUrl;
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username = "", string password = "", string returnUrl = "")
        {
            if (username == "" || password == "")
            {
                var tem = data.users.Where(m => m.username == username && m.password == password).FirstOrDefault();
                return RedirectToAction("Login", "Home", new { message = "Username and password must not empty." });
            }
            else
            {
                var tem = data.users.Where(m => m.username == username && m.password == password).FirstOrDefault();
                if (tem != null)
                {
                    Session["LoggedAccount"] = null;
                    AllLoggedUserInfo userFullInfo = new AllLoggedUserInfo(tem);
                    Session["LoggedAccount"] = userFullInfo;
                    string decodedUrl = "";
                    if (!string.IsNullOrEmpty(returnUrl))
                        decodedUrl = Server.UrlDecode(returnUrl);
                    if (decodedUrl != "" && !decodedUrl.Contains("Login"))
                    {
                        return Redirect(decodedUrl);
                    }
                    if (userFullInfo.role.RoleName == "super_admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Home", new { message = "Acount do not exist." });
                }
            }


        }
        public ActionResult Signup()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "signup");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            return View();
        }

        public ActionResult EditInfo(int? id)
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "editinfo");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            user user = data.users.Find(id);
            var userInfo = data.userdatas.Where(m => m.userid == id).FirstOrDefault();
            return View(userInfo);
        }
        public ActionResult SaveEditInfo(userdata model)
        {
            var user = data.userdatas.Find(model.Id);
            user.billing_address1 = model.billing_address1;
            user.billing_address2 = model.billing_address2;
            user.billing_country = model.billing_country;
            user.billing_email = model.billing_email;
            user.billing_name = model.billing_name;
            user.billing_phone = model.billing_phone;
            user.billing_poscode = model.billing_poscode;
            user.billing_state = model.billing_state;
            user.billing_suburb = model.billing_state;
            user.company_name = model.company_name;
            user.contact_email = model.contact_email;
            user.contact_phone = model.contact_phone;
            user.delivery_address1 = model.delivery_address1;
            user.delivery_address2 = model.delivery_address2;
            user.delivery_contry = model.delivery_contry;
            user.delivery_email = model.delivery_email;
            user.delivery_name = model.delivery_name;
            user.delivery_phone = model.delivery_phone;
            user.delivery_postcode = model.delivery_postcode;
            user.delivery_state = model.delivery_state;
            user.delivery_suburb = model.delivery_suburb;
            user.firstname = model.firstname;
            user.lasname = model.lasname;
            data.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Stores()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "store");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            var restult = data.stores;
            return View(restult);
        }
        public ActionResult Warehouses()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "warehouses");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            var restult = data.warehouses;
            return View(restult);
        }
        public ActionResult Products(SearchModel search, int? Page_No, int? Page_Size,bool? pagination)
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";
            if (pagination.HasValue && pagination.Value)
            {
                search = Session["SearchModel"]==null?new SearchModel():(SearchModel)Session["SearchModel"];
            }
            var seo = data.seos.FirstOrDefault(x => x.page == "home");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
       
       
            int defaSize = 20;
        
      
            if (Page_Size.HasValue)
            {
                defaSize = Page_Size ?? 20;
            }
            ViewBag.ValueToSet = defaSize;
            
            int No_Of_Page = (Page_No ?? 1);
            var _temp0 = search.ddlGetGroupno;
            var temp0 = search.ddlGetCategory;
            var temp1 = search.ddlPosition;
                var temp2 = search.AllKey==null?"": search.AllKey;
                var temp3 = search.ddlGetGroup == null ? "" : search.ddlGetGroup;// FormCollection["ddlGetGroup"];
                var temp4 = search.ddlGetMetaGroup == null ? "" : search.ddlGetMetaGroup;// FormCollection["ddlGetMetaGroup"];
                var temp5 = search.ddlGetItem == null ? "" : search.ddlGetItem;//FormCollection["ddlGetItem"];
                var temp6 = search.ddlGetDINCode == null ? "" : search.ddlGetDINCode;//FormCollection["ddlGetDINCode"];
               
                Session["SearchModel"] = search;
                var temp8 = search.ddlLength;//int.Parse(FormCollection["ddlLength"]);
                var temp9 = search.ddlGetInterFace;// int.Parse(FormCollection["ddlGetInterFace"]);
                var temp10 = search.ddlGetMaterial;// int.Parse(FormCollection["ddlGetMaterial"]);            
                var temp7 = search.ddlGetDimension;//int.Parse(FormCollection["ddlGetDimension"]);

               
                var query = from a in data.items
                            join b in data.artgrps on a.GROUPNO equals b.GROUPNO
                            join c in data.metagrups on b.METAGROUPNO equals c.METAGROUPNO
                            where (temp3 == "" || b.GROUPNAME.ToLower().Contains(temp3.ToLower()))
                            && (temp4 == "" || c.METAGROUPNAME.ToLower().Contains(temp4.ToLower()))
                            && (temp6 == "" || a.ARTCODE.ToLower().Contains(temp6.ToLower()))
                            && (temp2 == "" || a.ARTCODE.ToLower().Contains(temp2.ToLower()) || a.ARTNAME.ToLower().Contains(temp2.ToLower()))
                            &&(!temp8.HasValue || a.LEN==temp8)
                            &&(!temp0.HasValue || a.CATEGORYNO== temp0)
                            && (!_temp0.HasValue || a.GROUPNO == _temp0)
                            select a;


                
                if (temp1 == 1)
                {
                    query = query.OrderByDescending(m => m.ARTNAME);
                }
                else
                {
                    query = query.OrderBy(m => m.ARTNAME);
                }
              


                return View(query.ToPagedList(No_Of_Page, defaSize));
           


        }
        public ActionResult ExportToExcelStr(string token)
        {
            if (token.Equals("tudaika2019"))
            {
                //String constring = data.Database.Connection.ConnectionString;
                //constring += ";charset=utf8;convertzerodatetime=true;";

                //string file = Server.MapPath("/Content")+ "/backup.sql";
                //if (System.IO.File.Exists(file))
                //{
                //    System.IO.File.Delete(file);
                //}
                //System.IO.File.Create(file).Close();
                //using (MySqlConnection conn = new MySqlConnection(constring))
                //{
                //    using (MySqlCommand cmd = new MySqlCommand())
                //    {
                //        using (MySqlBackup mb = new MySqlBackup(cmd))
                //        {
                //            cmd.Connection = conn;
                //            conn.Open();                          
                //            mb.ImportFromFile(file);
                //            conn.Close();
                //        }
                //    }
                //}
               
                var query = from a in data.items select a;
                var gv = new GridView();
                gv.DataSource = query.ToList();
                gv.DataBind();
                
                Response.ClearContent();
                Response.Buffer = true;
                String dateStr = DateTime.Now.ToString("yyyy-MM-dd_HH:mm");
                Response.AddHeader("content-disposition", "attachment; filename=" + dateStr + "_Products.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                data.Database.ExecuteSqlCommand("delete  from item");
                new Thread(() => {
                    Thread.Sleep(5000);
                   // System.IO.File.Delete(file);
                }).Start();
                return View("Index");//new FileStreamResult(new FileStream(file,FileMode.Open), "application/octet-stream");

            }
            return RedirectToAction("Index");
        }
        public ActionResult ExportToExcel()
        {
           var  search = Session["SearchModel"] == null ? new SearchModel() : (SearchModel)Session["SearchModel"];

            var _temp0 = search.ddlGetGroupno;
            var temp0 = search.ddlGetCategory;
            var temp1 = search.ddlPosition;
            var temp2 = search.AllKey == null ? "" : search.AllKey;
            var temp3 = search.ddlGetGroup == null ? "" : search.ddlGetGroup;// FormCollection["ddlGetGroup"];
            var temp4 = search.ddlGetMetaGroup == null ? "" : search.ddlGetMetaGroup;// FormCollection["ddlGetMetaGroup"];
            var temp5 = search.ddlGetItem == null ? "" : search.ddlGetItem;//FormCollection["ddlGetItem"];
            var temp6 = search.ddlGetDINCode == null ? "" : search.ddlGetDINCode;//FormCollection["ddlGetDINCode"];

         
            var temp8 = search.ddlLength;//int.Parse(FormCollection["ddlLength"]);
            var temp9 = search.ddlGetInterFace;// int.Parse(FormCollection["ddlGetInterFace"]);
            var temp10 = search.ddlGetMaterial;// int.Parse(FormCollection["ddlGetMaterial"]);            
            var temp7 = search.ddlGetDimension;//int.Parse(FormCollection["ddlGetDimension"]);


            var query = from a in data.items
                        join b in data.artgrps on a.GROUPNO equals b.GROUPNO
                        join c in data.metagrups on b.METAGROUPNO equals c.METAGROUPNO
                        where (temp3 == "" || b.GROUPNAME.ToLower().Contains(temp3.ToLower()))
                        && (temp4 == "" || c.METAGROUPNAME.ToLower().Contains(temp4.ToLower()))
                        && (temp6 == "" || a.ARTCODE.ToLower().Contains(temp6.ToLower()))
                        && (temp2 == "" || a.ARTCODE.ToLower().Contains(temp2.ToLower()) || a.ARTNAME.ToLower().Contains(temp2.ToLower()))
                        && (!temp8.HasValue || a.LEN == temp8)
                        && (!temp0.HasValue || a.CATEGORYNO == temp0)
                        && (!_temp0.HasValue || a.GROUPNO == _temp0)
                        select new ExportProduct {  MetaGroup=c.METAGROUPNAME,Group=b.GROUPNAME,Name=a.ARTNAME,DinCode=a.ARTCODE,Description=a.INFO,Height=a.HEIGHT,Length=a.LEN,Weight=a.WEIGHT,Width=a.WIDTH,Unit=a.UNITNAME,UnitDescription=a.UNITDESCR};



            if (temp1 == 1)
            {
                query = query.OrderByDescending(m => m.Name);
            }
            else
            {
                query = query.OrderBy(m => m.Name);
            }

            var gv = new GridView();
            gv.DataSource = query.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            String dateStr = DateTime.Now.ToString("yyyy-MM-dd_HH:mm");
            Response.AddHeader("content-disposition", "attachment; filename="+ dateStr + "_Products.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
        public ActionResult Download()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "download");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            return View(data.files.Where(m => m.Status == "Active").ToList());
        }

        public ActionResult Store()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "store");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            return View();
        }
        public ActionResult UpdateCart(string[] chkbox, int[] qty, string[] code, float[] price,string[] sName,int[] sNo, int? Page_No, int? Page_Size)
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "updatecart");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            /*if (chkbox == null && code == null && price == null && Session["ShoppingCart"]==null)
            {
                if (chkbox == null)
                {
                    return RedirectToAction("BulkProducts", "Home");
                }
                else
                {
                    return RedirectToAction("Products", "Home");
                }
            }*/



            WebApplication1.Models.AllLoggedUserInfo userFullInfo = (WebApplication1.Models.AllLoggedUserInfo)Session["LoggedAccount"];


            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var pro = (promotion)null;
            if (userFullInfo != null && userFullInfo.role.Id == 5)
            {
                var user = data.users.Find(userFullInfo.user.Id);
                string type = user.type + "";
                pro = data.promotions.Where(t => t.STATUS == 1 && t.TYPEUSERS.Contains(type) && t.FIRSTDATE <= now && now <= t.LASTDATE).FirstOrDefault();
            }
            ShoppingCart Cart = new ShoppingCart();
            if (Session["ShoppingCart"] != null)
            {
                Cart = (ShoppingCart)Session["ShoppingCart"];

            }
            if (pro != null)
            {
                Cart.promotion = pro;
            }
            if (chkbox == null && qty != null)//for prodetail and update cart
            {
                List<CartItem> cartItems = Cart.cartItem;
                for (int i = 0; i < qty.Count(); i++)
                {
                    var temItem = cartItems.Where(m => m.Code == code[i]).FirstOrDefault();
                    if (temItem == null) //not exist in car => add new cartitem
                    {
                        CartItem newline = new CartItem();
                        newline.Code = code[i];
                        newline.Qty = qty[i];
                        var codename = code[i];
                        newline.Name = data.items.Where(m => m.ARTCODE == codename).FirstOrDefault().ARTNAME;
                        newline.Price = price[i];
                        newline.LineTotal = qty[i] * price[i];
                        newline.Tax = (qty[i] * price[i]) * 0.1f;
                        try
                        {
                            newline.StockName = sName[i];
                            newline.StockId = sNo[i];
                          

                        }
                        catch (Exception)
                        {

                            
                        }

                        Cart.cartItem.Add(newline);
                    }
                    else // already existed in cart => update item info
                    {
                        Cart.cartItem[i].Qty = qty[i];
                        Cart.cartItem[i].Price = price[i];
                        Cart.cartItem[i].LineTotal = qty[i] * price[i];
                        Cart.cartItem[i].Tax = (qty[i] * price[i]) * 0.1f;
                    }
                }
                Cart.taxTotal = Cart.cartItem.Sum(m => m.Tax);
                Cart.CartTotal = Cart.cartItem.Sum(m => m.LineTotal);
                if (Cart.promotion != null)
                {
                    if (Cart.promotion.TYPENO == 0)
                    {
                        Cart.PromotionTotal = Cart.CartTotal * float.Parse(Cart.promotion.VALUEPROMOTION) / 100;
                        Cart.CartTotal = Cart.CartTotal * (100 - float.Parse(Cart.promotion.VALUEPROMOTION)) / 100;
                    }
                    else if (Cart.promotion.TYPENO == 1)
                    {
                        try
                        {
                            Cart.PromotionTotal = float.Parse(Cart.promotion.VALUEPROMOTION);
                            Cart.CartTotal -= float.Parse(Cart.promotion.VALUEPROMOTION);
                        }
                        catch (Exception)
                        {


                        }

                    }
                }


            }
            if (chkbox != null && qty != null) // for add or update bundle item
            {
                List<CartItem> cartItems = Cart.cartItem;
                for (int i = 0; i < chkbox.Count(); i++)
                {
                    var getDate = chkbox[i].Split('|');
                    var codeCheckbox = getDate[0];
                    var priceCheckbox = getDate[1];
                    var indexCheckbox = int.Parse(getDate[2]);
                    var temItem = cartItems.Where(m => m.Code == codeCheckbox).FirstOrDefault();
                    if (temItem == null) //not exist in car => add new cartitem
                    {
                        CartItem newline = new CartItem();
                        newline.Code = codeCheckbox;
                        newline.Qty = qty[indexCheckbox];
                        newline.Name = data.items.Where(m => m.ARTCODE == codeCheckbox).FirstOrDefault().ARTNAME;
                        newline.Price = string.IsNullOrEmpty(priceCheckbox) ? 0 : float.Parse(priceCheckbox);
                        newline.LineTotal = newline.Qty * newline.Price;
                        newline.Tax = newline.LineTotal * 0.1f;
                        try
                        {
                            newline.StockName = sName[i];
                            newline.StockId = sNo[i];
                        }
                        catch (Exception)
                        {


                        }
                        Cart.cartItem.Add(newline);
                    }
                    else // already existed in cart => update item info
                    {
                        Cart.cartItem[i].Qty = qty[indexCheckbox];
                        Cart.cartItem[i].Price = string.IsNullOrEmpty(priceCheckbox) ? 0 : float.Parse(priceCheckbox);
                        Cart.cartItem[i].LineTotal = Cart.cartItem[i].Qty * Cart.cartItem[i].Price;
                        Cart.cartItem[i].Tax = Cart.cartItem[i].LineTotal * 0.1f;
                    }
                }
                Cart.taxTotal = Cart.cartItem.Sum(m => m.Tax);
                Cart.CartTotal = Cart.cartItem.Sum(m => m.LineTotal);
                if (Cart.promotion != null)
                {
                    if (Cart.promotion.TYPENO == 0)
                    {
                        Cart.PromotionTotal = Cart.CartTotal * float.Parse(Cart.promotion.VALUEPROMOTION) / 100;
                        Cart.CartTotal = Cart.CartTotal * (100 - float.Parse(Cart.promotion.VALUEPROMOTION)) / 100;
                    }
                    else if (Cart.promotion.TYPENO == 1)
                    {
                        try
                        {
                            Cart.PromotionTotal = float.Parse(Cart.promotion.VALUEPROMOTION);
                            Cart.CartTotal -= float.Parse(Cart.promotion.VALUEPROMOTION);
                        }
                        catch (Exception)
                        {


                        }

                    }
                }
            }
            var listItemCode = Cart.cartItem.Select(t => t.Code);
            var listItem = data.items.Where(t=> listItemCode.Contains(t.ARTCODE)).ToList();
            var listIDStock = Cart.cartItem.Where(t => t.StockId > 0).Select(t => t.StockId).ToList();
            var listStock = data.stocks.Where(t=> listIDStock.Contains(t.STOCKNO)).ToList();
            foreach (var item in Cart.cartItem)
            {
                if (item.StockId>0 && !string.IsNullOrWhiteSpace( item.Code))
                {
                    try
                    {
                        var _item = listItem.FirstOrDefault(t => t.ARTCODE == item.Code);
                        var cStock = listStock.Where(t => t.ARTNO == _item.ARTNO && t.STOCKNO == item.StockId);
                        item.StockMaxValue = (int)cStock.Sum(t => t.VOLUME);
                    }
                    catch (Exception)
                    {

                       
                    }
                }
               


            }
            List<CartItem> listItemForPaging = new List<CartItem>();
            int No_Of_Page = (Page_No ?? 1);
            int defaSize = 10;
            if (Cart.cartItem.Count() > 0)
            {
                Session["ShoppingCart"] = Cart;

                if (Page_Size != null)
                {
                    defaSize = Page_Size ?? 10;
                }
                ViewBag.ValueToSet = defaSize;

                listItemForPaging = Cart.cartItem;
            }
            else
            {
                Session["ShoppingCart"] = null;
            }
            return View(listItemForPaging.ToPagedList(No_Of_Page, defaSize));
        }
        public ActionResult RemoveCartItem(string code)
        {
            ShoppingCart Cart = new ShoppingCart();
            if (Session["ShoppingCart"] != null)
            {
                Cart = (ShoppingCart)Session["ShoppingCart"];
            }
            var removedItem = Cart.cartItem.Where(m => m.Code == code).FirstOrDefault();
            Cart.cartItem.Remove(removedItem);
            return RedirectToAction("UpdateCart", "Home");
        }
        public ActionResult ClearCart()
        {
            ShoppingCart Cart = new ShoppingCart();
            Session["ShoppingCart"] = null;
            return RedirectToAction("UpdateCart", "Home");
        }
        public ActionResult Checkout()
        {/*
            string currencyName = "USD";
            ordersetting ordersetting=  data.ordersettings.Where(t => t.status == 1).FirstOrDefault();
            if (ordersetting!=null)
            {
                currencyName = ordersetting.name;
            }
            string randomKey = "";
            var paypayconfig = new PayPalConfiguration();
            var apiContext = paypayconfig.GetAPIContext();
            string paypalURL = "";
            double cartAmount = 0;
            var itemList = new ItemList();
            var items = new List<Item>();
            double amt = 0;
            var Item = new Item();
            Item.name = "Subscription";
            Item.currency = currencyName;
            Item.price = amt.ToString();
            Item.quantity = "1";
            items.Add(Item);
            cartAmount += amt;

            itemList.items = items;
            cartAmount = Math.Round(cartAmount, 2);

            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = UrlHelper.Root + "Home/Checkout?" + UrlHelper.ToQueryString(new { type = "" }),
                return_url = UrlHelper.Root + "Home/CheckoutSuccess?" + UrlHelper.ToQueryString(new { randomkey = randomKey, type = "" })
            };
            var paypalAmount = new Amount() { currency = currencyName, total = cartAmount.ToString() };

            var transactionList = new List<PayPal.Api.Transaction>();
            PayPal.Api.Transaction transaction = new PayPal.Api.Transaction();
            transaction.amount = paypalAmount;
            transaction.item_list = itemList;
            transactionList.Add(transaction);


            var payment = new Payment()
            {
                intent = "Sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            try
            {
                var createdPayment = payment.Create(apiContext);
                var links = createdPayment.links.GetEnumerator();
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalURL = link.href;
                    }
                }

            }
            catch (PaymentsException ex)
            {
                paypalURL = "ERROR: " + ex.Response;
            }
            */


            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string type)
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "about");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.MetaTitle = "Contact";
            ViewBag.MetaDescription = "Contact - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "contact");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult GetGroup()
        {
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.ToList();
            SelectList obg = new SelectList(obj, "METAGROUPNO", "METAGROUPNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMetaGroup()
        {
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.ToList();
            SelectList obg = new SelectList(obj, "METAGROUPNO", "METAGROUPNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMetaGroupMenu(int mastermetagroupid)
        {
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.Where(m => m.PARENTNO == mastermetagroupid).ToList();
            SelectList obg = new SelectList(obj, "METAGROUPNO", "METAGROUPNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MasterGroup()
        {
            ViewBag.MetaTitle = "MasterGroup";
            ViewBag.MetaDescription = "MasterGroup - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "mastergroup");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.Where(m => m.PARENTNO == 0 && m.CodeLanguage==lang.ToLower()).ToList();
            return View(obj);
        }
        public ActionResult MetaGroup(int masterGroupID)
        {
            ViewBag.MetaTitle = "Contact";
            ViewBag.MetaDescription = "Contact - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "metagroup");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.Where(m => m.PARENTNO == masterGroupID && m.IsActive == true && m.CodeLanguage== lang.ToLower()).ToList();
            Session["MasterGroup"] = data.metagrups.Where(m => m.METAGROUPNO == masterGroupID).FirstOrDefault().METAGROUPNAME;
            return View(obj);
        }
        public ActionResult GetMasterGroup()
        {
            List<metagrup> obj = new List<metagrup>();
            obj = data.metagrups.Where(m => m.PARENTNO == 0).ToList();
            SelectList obg = new SelectList(obj, "METAGROUPNO", "METAGROUPNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCatalogue(int metagroupId)
        {
            List<catalogue> obj = new List<catalogue>();
            obj = data.catalogues.Where(m => m.MetaGroupId == metagroupId).ToList();
            SelectList obg = new SelectList(obj, "CatalogueCode", "CatalogueName", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGroupMenu(int metagroupId)
        {
            List<artgrp> obj = new List<artgrp>();
            obj = data.artgrps.Where(m => m.METAGROUPNO == metagroupId).ToList();
            SelectList obg = new SelectList(obj, "GroupNo", "GroupName", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Catalogue(int metagroupId)
        {
            ViewBag.MetaTitle = "Contact";
            ViewBag.MetaDescription = "Contact - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "home");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            List<catalogue> obj = new List<catalogue>();
            obj = data.catalogues.Where(m => m.MetaGroupId == metagroupId).ToList();
            Session["Catalogue"] = data.metagrups.Where(m => m.METAGROUPNO == metagroupId).FirstOrDefault().METAGROUPNAME;
            return View(obj);
        }
        public ActionResult Group(int metagroupId)
        {
            List<artgrp> obj = new List<artgrp>();
            obj = data.artgrps.Where(m => m.METAGROUPNO == metagroupId).ToList();
            Session["Group"] = data.metagrups.Where(m => m.METAGROUPNO == metagroupId).FirstOrDefault().METAGROUPNAME;
            return View(obj);
        }
        public ActionResult GetCategory()
        {
            List<category> obj = new List<category>();
            obj = data.categories.ToList();
            SelectList obg = new SelectList(obj, "CATEGORYNO", "CATEGORYNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Category(int CatId)
        {
            ViewBag.MetaTitle = "Contact";
            ViewBag.MetaDescription = "Contact - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "category");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            List<category> obj = new List<category>();
            obj = data.categories.Where(m => m.CatalogueCode == CatId).ToList();
            return View(obj);
        }

        public ActionResult GetCategoryByCatId(int CatId)
        {
            List<category> obj = new List<category>();
            obj = data.categories.Where(m => m.CatalogueCode == CatId).ToList();
            SelectList obg = new SelectList(obj, "CATEGORYNO", "CATEGORYNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateCatalogue()
        {
            ViewBag.MetaTitle = "Contact";
            ViewBag.MetaDescription = "Contact - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = data.seos.FirstOrDefault(x => x.page == "createcatalogue");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            return View();
        }
        public ActionResult GetItem()
        {
            List<item> obj = new List<item>();
            obj = data.items.Take(1000).ToList();
            SelectList obg = new SelectList(obj, "ARTCODE", "ARTNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(int catId)
        {
            List<item> obj = new List<item>();
            obj = data.items.Where(m => m.CATEGORYNO == catId).Take(1000).ToList();
            SelectList obg = new SelectList(obj, "ARTCODE", "ARTNAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDimension()
        {
            List<artdimension> obj = new List<artdimension>();
            obj = data.artdimensions.Take(1000).ToList();
            SelectList obg = new SelectList(obj, "ARTCODE", "WIDTH", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDINCode()
        {
            List<barcode> obj = new List<barcode>();
            obj = data.barcodes.Take(1000).ToList();
            SelectList obg = new SelectList(obj, "ARTNO", "BARCODE1", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLength()
        {
            List<artdimension> obj = new List<artdimension>();
            obj = data.artdimensions.Take(1000).ToList();
            SelectList obg = new SelectList(obj, "ARTCODE", "LEN", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInterface()
        {
            List<artname> obj = new List<artname>();
            obj = data.artnames.Take(1000).ToList();
            SelectList obg = new SelectList(obj, "LANGNO", "NAME", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FileList()
        {
            return PartialView("Partial1", data.files.ToList());
        }

        /*public ActionResult GetMaterial()
        {
            List<artdimension> obj = new List<artdimension>();
            obj = data..ToList();
            SelectList obg = new SelectList(obj, "ARTCODE", "LEN", 0);
            return Json(obg, JsonRequestBehavior.AllowGet);
        }*/



        //////////Search//////////
        public ActionResult _ProductAjaxAutoComplete(string query)
        {
            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var list = data.items.Where(x=>x.CodeLanguage==lang).Join(data.artgrps, it => it.GROUPNO, grp => grp.GROUPNO, (it, grp) => new AllModel { tblitem = it, tblGroup = grp }).Where(x => x.tblitem.ARTNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTNAME, des = x.tblitem.INFO, grp = x.tblGroup.GROUPNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _MetagrupAjaxAutoComplete(string query)
        {

            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var list = data.metagrups.Select(x => new AllModel { tblMetaGroup = x }).Where(x =>x.tblMetaGroup.CodeLanguage==lang &&  x.tblMetaGroup.PARENTNO == 0 && x.tblMetaGroup.METAGROUPNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblMetaGroup.METAGROUPNO, label = x.tblMetaGroup.METAGROUPNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GroupAjaxAutoComplete(string query)
        {

            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var list = data.artgrps.Select(x => new AllModel { tblGroup = x }).Where(x =>x.tblGroup.CodeLanguage==lang && x.tblGroup.GROUPNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblGroup.GROUPNO, label = x.tblGroup.GROUPNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ItemAjaxAutoComplete(string query)
        {

            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var list = data.items.Select(x => new AllModel { tblitem = x }).Where(x => x.tblitem.CodeLanguage == lang && x.tblitem.ARTNAME.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTNAME }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DinAjaxAutoComplete(string query)
        {

            var lang = "";
            var httpCookie = Request.Cookies["Language"];
            if (httpCookie != null)
            {
                lang = httpCookie.Value;
            }
            else
            {
                lang = "english";
            }
            var list = data.items.Select(x => new AllModel { tblitem = x }).Where(x => x.tblitem.CodeLanguage == lang && x.tblitem.ARTCODE.ToLower().Contains(query.ToLower())).ToList().Select(x => new { value = x.tblitem.ARTNO, label = x.tblitem.ARTCODE }).ToList();
            return Json(new { status = true, data = list }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetLanguage(string lan = null)
        {
            var dataVo = data.vocabularies.ToList();
            if (lan != null)
            {
                dataVo = dataVo.Where(x => x.language.ToLower() == lan.ToLower()).ToList();


            }
            else
            {
                dataVo = dataVo.Where(x => x.language.ToLower() == "english").ToList();
            }
            return Json(dataVo.ToList());



        }
    }
}