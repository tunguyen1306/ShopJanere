using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using CKFinder.Connector;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        veebdbEntities db = new veebdbEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckUser(string email, int type)
        {
            var ck = false;
            if (type == 1)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var dataUser = db.users.ToList().FirstOrDefault(x => x.email.ToLower() == email.ToLower());
                    if (dataUser != null)
                    {
                        ck = true;
                    }
                }
                else
                {
                    ck = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var dataUser = db.users.ToList().FirstOrDefault(x => x.username.ToLower() == email.ToLower());
                    if (dataUser != null)
                    {
                        ck = true;
                    }
                }
                else
                {
                    ck = true;
                }
            }

            return Json(new { result = ck });
        }
        public ActionResult SaveSignup(user user)
        {
            user.status = "active";
            user.createdate = DateTime.Now;
            user.updatedate = DateTime.Now;
            user.type = 1;
            user.discount = 0;
            db.users.Add(user);
            db.SaveChanges();
            user_role ur = new user_role();
            ur.userid = user.Id;
            ur.roleid = 6;
            ur.status = "active";
            db.user_role.Add(ur);
            userdata ud = new userdata();
            ud.userid = user.Id;
            db.userdatas.Add(ud);
            db.SaveChanges();
            var t = SendTemplateEmail(user.email, user.email, "", "Email register success", 1);
            var tem = db.users.FirstOrDefault(m => m.username == user.username && m.password == user.password);
            if (tem != null)
            {
                Session["LoggedAccount"] = null;
                AllLoggedUserInfo userFullInfo = new AllLoggedUserInfo(tem);
                Session["LoggedAccount"] = userFullInfo;
                string decodedUrl = "";
                if (!string.IsNullOrEmpty(Url.Action("AccountInfo", "Account", new { id = user.Id })))
                    decodedUrl = Server.UrlDecode(Url.Action("AccountInfo", "Account", new { id = user.Id }));
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
        [HttpPost]
        public ActionResult FogotPass(string emailFogot)
        {
            var re = 0;
            var tblUser = db.users.FirstOrDefault(x => x.email == emailFogot);
            if (tblUser != null)
            {
                var t = SendTemplateEmail(emailFogot, emailFogot, "", "Email retrieve password", 2);
                re = t ? 1 : 2;
            }
            else
            {
                re = 3;
            }
            return Json(new { result = re });

        }
        public bool SendTemplateEmail(string recepientEmail, string username, string key, string Subject, int type)
        {
            bool t = false;
            //Type =1 Active Email
            //Type =2 ForgetPass
            string body = string.Empty;
            var activelink = "";
            if (type == 1)
            {
                //activelink = ConfigurationManager.AppSettings["UrlWeb"] + "/BDSAccounts/ActiveAccount/?token=" + key;
                body = ViewRenderer.RenderPartialView("~/Views/Shared/Partial/_ActiveTemplateMail.cshtml");
                body = body.Replace("##name##", username);
                //body = body.Replace("##activatelink##", activelink);
            }
            if (type == 2)
            {
                var check = db.users.ToList().FirstOrDefault(x => x.email == recepientEmail.ToLower());
                if (check != null)
                {
                    string securityCode = check.password;
                    var keyPass = securityCode + "##" + check.Id;
                    var md5Hash = MD5.Create();
                    var keyPassword = Models.Helper.GetMd5Hash(md5Hash, keyPass);

                    activelink = string.Format(ConfigurationManager.AppSettings["UrlWeb"] + "Account/EmailFogotPass?mb={0}&code={1}", check.Id, keyPassword);

                    body = ViewRenderer.RenderPartialView("~/Views/Shared/Partial/_ResetPassTemplateMail.cshtml");
                    body = body.Replace("##name##", username);
                    body = body.Replace("##activatelink##", activelink);

                }
            }
            if (type == 3)
            {




                body = key;


            }

            t = Models.Helper.SendEmail("donotreply@example.com", recepientEmail, Subject, body);


            return t;
        }

        [HttpPost]
        public ActionResult CheckCapcha(string response)
        {
            var registerEmp = "6LdfXUQUAAAAALvfd3CzktaouXRrQHy6jV1-9LAW";

            var secretKey = "";
            secretKey = registerEmp;


            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return Json(new { result = status });
        }
        public ActionResult EmailFogotPass(int mb, string code)
        {
            string msg = "";

            ViewBag.Notification = -1;
            ViewBag.ErrorKey = -1;

            var member = db.users.FirstOrDefault(x => x.Id == mb);

            if (member != null)
            {
                var keyPass = member.password + "##" + member.Id;
                var md5Hash = MD5.Create();
                var keyPassword = Models.Helper.GetMd5Hash(md5Hash, keyPass);
                if (code == keyPassword)
                {

                    return View(member);
                }
                else
                {
                    ViewBag.ErrorKey = 0;
                }

            }

            return View(member);



        }
        [HttpPost]
        public ActionResult EmailFogotPass(user model)
        {
            var messenge = 0;
            var tblUser = db.users.Find(model.Id);
            if (tblUser != null)
            {
                tblUser.password = model.password;
                db.Entry(tblUser).State = EntityState.Modified;
                db.SaveChanges();
                messenge = 1;


            }

            ViewBag.Notification = messenge;
            return View(tblUser);

        }
        public ActionResult AccountInfo(int? id)
        {
            ViewBag.Notification = -1;
            ViewBag.MetaTitle = "Home";
            ViewBag.MetaDescription = "Home - janere";
            ViewBag.Link = "http://shop.janere.ee/";
            ViewBag.Keyword = "janere";

            var seo = db.seos.FirstOrDefault(x => x.page == "editinfo");
            if (seo != null)
            {
                ViewBag.MetaTitle = seo.title;
                ViewBag.MetaDescription = seo.description;
                ViewBag.Link = seo.link;
                ViewBag.Keyword = seo.keyword;
            }
            var data = (from us in db.users
                        join usdt in db.userdatas on us.Id equals usdt.userid
                        where us.Id == id
                        select new AllModel { tblUser = us, tblUserData = usdt }).FirstOrDefault();
            return View(data);
        }
        public ActionResult ChangePass(int id)
        {

            ViewBag.Notification = -1;
            var member = db.users.FirstOrDefault(x => x.Id == id);

            if (member != null)
            {

                return View(member);
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        public ActionResult ChangePass(user model)
        {
            var messenge = 0;
            var tblUser = db.users.Find(model.Id);
            if (tblUser != null)
            {
                tblUser.password = model.password;
                db.Entry(tblUser).State = EntityState.Modified;
                db.SaveChanges();
                messenge = 1;


            }

            ViewBag.Notification = messenge;
            return View(model);

        }
        public ActionResult OrderHistory(int id)
        {

            var listOrderStatus = db.orderstatus.ToList();
            listOrderStatus.Insert(0, new orderstatu { id = 0, name = "Select Action" });
            ViewBag.OrderStatus = listOrderStatus;
            ViewBag.IdUser = id;

            return View();

        }
        public ActionResult OrderHistoryAjax(int id, string datepicker = null, string searchOrder = null, int start = 0, int view = 10)
        {

            var listOrder = db.orders.Where(x => x.clientID == id);


            var listAll = (from pro in listOrder
                           select new { tblOrder = pro });

            if (!string.IsNullOrEmpty(datepicker))
            {
                var date = DateTime.ParseExact(datepicker, "dd/MM/yyyy", null);
                listAll = listAll.Where(x => x.tblOrder.submitDate <= date);
            }
            if (searchOrder != null)
            {
                listAll = listAll.Where(x => x.tblOrder.ocid.ToString().Contains(searchOrder) || x.tblOrder.d_addr1.ToString().Contains(searchOrder) || x.tblOrder.b_fname.ToString().Contains(searchOrder) || x.tblOrder.b_lname.ToString().Contains(searchOrder));
            }
            var count = listAll.Count();
            ViewBag.Start = start;
            ViewBag.View = view;
            ViewBag.Total = count;
            ViewBag.ViewOf = count;
            var dbData = listAll.OrderBy(t => t.tblOrder.ocid).Skip(start).Take(view).OrderByDescending(x => x.tblOrder.status).ToList();


            var datas = dbData.Select(t => new AllModel { tblOrder = t.tblOrder }).ToList();
            return PartialView(datas);


        }

        [HttpPost]
        public ActionResult AccountInfo(AllModel model)
        {
            var user = db.users.Find(model.tblUser.Id);
            if (user != null)
            {
                ViewBag.Notification = 0;
                user.updatedate = DateTime.Now;
                user.display = model.tblUser.display;
                user.email = model.tblUser.email;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                var userdata = db.userdatas.Find(model.tblUserData.Id);
                if (userdata != null)
                {
                    userdata.firstname = model.tblUserData.firstname;
                    userdata.lasname = model.tblUserData.lasname;
                    userdata.contact_email = model.tblUserData.contact_email;
                    userdata.contact_phone = model.tblUserData.contact_phone;
                    userdata.company_name = model.tblUserData.company_name;
                    userdata.delivery_name = model.tblUserData.delivery_name;
                    userdata.delivery_email = model.tblUserData.delivery_email;
                    userdata.delivery_phone = model.tblUserData.delivery_phone;
                    userdata.delivery_address1 = model.tblUserData.delivery_address1;
                    userdata.delivery_address2 = model.tblUserData.delivery_address2;
                    userdata.delivery_suburb = model.tblUserData.delivery_suburb;
                    userdata.delivery_postcode = model.tblUserData.delivery_postcode;
                    userdata.delivery_state = model.tblUserData.delivery_state;
                    userdata.delivery_contry = model.tblUserData.delivery_contry;
                    userdata.billing_name = model.tblUserData.billing_name;
                    userdata.billing_email = model.tblUserData.billing_email;
                    userdata.billing_phone = model.tblUserData.billing_phone;
                    userdata.billing_address1 = model.tblUserData.billing_address1;
                    userdata.billing_address2 = model.tblUserData.billing_address2;
                    userdata.billing_suburb = model.tblUserData.billing_suburb;
                    userdata.billing_poscode = model.tblUserData.billing_poscode;
                    userdata.billing_state = model.tblUserData.billing_state;
                    userdata.billing_country = model.tblUserData.billing_country;
                    db.Entry(userdata).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Notification = 1;
                }
                else
                {
                    ViewBag.Notification = 0;
                }

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SendAskForPrice(string email, string name, string phone, string qty, string productId)
        {
            var re = 0;
            var content = "";
            var namePro = "";
            if (email != null)
            {
                content += "Email: " + email + "<br>";
                content += "Name: " + name + "<br>";
                content += "Phone: " + phone + "<br>";
                content += "Quantity: " + qty + "<br>";
                if (productId != null)
                {
                    var proID = int.Parse(productId);
                    var tblProduct = db.items.FirstOrDefault(x => x.ARTNO == proID);
                    if (tblProduct != null)
                    {
                        content += "Code:" + tblProduct.ARTCODE + ", Product:" + tblProduct.ARTNAME;
                        namePro = "Code:" + tblProduct.ARTCODE + ", Product:" + tblProduct.ARTNAME;
                    }
                }
                var t = SendTemplateEmail(email, email, content, "Email Ask For Price", 3);
                var tblEmail = new email
                {
                    email1 = email,
                    phone = phone,
                    name = name,
                    nameproduct = namePro,
                    status = 1,
                    type = 1,
                    responedate = DateTime.Now,
                    createdate = DateTime.Now,
                    quantity = qty
                };
                db.emails.Add(tblEmail);
                db.SaveChanges();


                re = t ? 1 : 3;
            }

            return Json(new { result = re });

        }

        public ActionResult CloneLanguage(int type)
        {
            var listLanguage = db.countries.Where(x => x.status == 1 && x.islanguage == 1).ToList();
            var index = 0;
            if (type == 1)
            {
                var listPro = db.items.ToList();

                foreach (var proItem in listPro)
                {
                    foreach (var itemLang in listLanguage)
                    {
                        var itempro = db.items.FirstOrDefault(x => x.IdCurrentItem == proItem.IdCurrentItem && x.CodeLanguage == itemLang.language.ToLower());

                        if (itempro == null)
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
                            if (tblLink != null)
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
                            index++;
                        }
                        if (index == 25)
                        {
                            index = 0;
                            db.SaveChanges();
                        }

                    }


                }



            }
            if (type == 2)
            {

                var listGroup = db.artgrps.ToList();
                foreach (var itemGroup in listGroup)
                {
                    CloneDQGroup(itemGroup.IdCurrentItem, listLanguage);
                }
            }
            if (type == 3)
            {
                var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO == 0).ToList();
                foreach (var itemGroup in listMasterMetaGroup)
                {
                    CloneDqMasterMetaGroup(itemGroup.IdCurrentItem, listLanguage);
                }

            }
            if (type == 4)
            {
                var listMasterMetaGroup = db.metagrups.Where(x => x.PARENTNO != 0).ToList();
                foreach (var itemGroup in listMasterMetaGroup)
                {
                    CloneDqMetaGroup(itemGroup.METAGROUPNO, listLanguage);
                }

            }

            return null;

        }




        public void CloneDQGroup(int? currentId, List<country> listLanguage)
        {
            var index = 0;
            var proMaster = db.artgrps.FirstOrDefault(x => x.GROUPNO == currentId);
            if (proMaster != null)
            {
                foreach (var itemlang in listLanguage)
                {
                    if (proMaster.CodeLanguage != itemlang.language.ToLower())
                    {
                        var check = db.artgrps.FirstOrDefault(x => x.IdCurrentItem == currentId && x.CodeLanguage == itemlang.language.ToLower());
                        if (check == null)
                        {
                            var tblgroup = new artgrp
                            {
                                CREATED = proMaster.CREATED,
                                LASTCHANGE = proMaster.LASTCHANGE,
                                GROUPCODE = proMaster.GROUPCODE,
                                GROUPNAME = proMaster.GROUPNAME,
                                IsActive = proMaster.IsActive,
                                EXPORTABLE = proMaster.EXPORTABLE,
                                METAGROUPNO = proMaster.METAGROUPNO,
                                CodeLanguage = itemlang.language.ToLower(),
                                IdCurrentItem = proMaster.IdCurrentItem,

                            };
                            db.artgrps.Add(tblgroup);

                        }


                    }




                }
                db.SaveChanges();

            }

        }



        public void CloneDqMasterMetaGroup(int? currentId, List<country> listLanguage)
        {
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPNO == currentId);
            var index = 0;
            if (proMaster != null)
            {
                foreach (var itemlang in listLanguage)
                {
                    if (proMaster.CodeLanguage != itemlang.language.ToLower())
                    {

                        var check = db.metagrups.FirstOrDefault(x => x.IdCurrentItem == currentId && x.CodeLanguage == itemlang.language.ToLower());
                        if (check == null)
                        {
                            var matagroup = new metagrup
                            {
                                PARENTNO = proMaster.PARENTNO,
                                IsActive = proMaster.IsActive,
                                CREATED = proMaster.CREATED,
                                METAGROUPNAME = proMaster.METAGROUPNAME,
                                METAGROUPCODE = proMaster.METAGROUPCODE,
                                CodeLanguage = itemlang.language.ToLower(),
                                IdCurrentItem = proMaster.METAGROUPNO
                            };
                            db.metagrups.Add(matagroup);

                        }
                    }



                }

                db.SaveChanges();
            }

        }



        public void CloneDqMetaGroup(int currentId, List<country> listLanguage)
        {
            var index = 0;
            var proMaster = db.metagrups.FirstOrDefault(x => x.METAGROUPNO == currentId);
            if (proMaster != null)
            {
                foreach (var itemlang in listLanguage)
                {
                    if (proMaster.CodeLanguage != itemlang.language.ToLower())
                    {
                        var check = db.metagrups.FirstOrDefault(x => x.IdCurrentItem == currentId && x.CodeLanguage == itemlang.language.ToLower());
                        if (check == null)
                        {
                            var matagroup = new metagrup
                            {
                                PARENTNO = proMaster.PARENTNO,
                                IsActive = proMaster.IsActive,
                                CREATED = proMaster.CREATED,
                                METAGROUPNAME = proMaster.METAGROUPNAME,
                                METAGROUPCODE = proMaster.METAGROUPCODE,
                                CodeLanguage = itemlang.language.ToLower(),
                                IdCurrentItem = proMaster.METAGROUPNO
                            };
                            db.metagrups.Add(matagroup);

                        }
                    }




                }
                db.SaveChanges();

            }

        }

    }
}