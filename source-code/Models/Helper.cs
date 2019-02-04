using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1.Models
{
    public static class Helper
    {
        public static readonly Dictionary<int, string> RoleList
        = new Dictionary<int, string>
        {
            { 1, "super_admin" },
            { 2, "sale_manager" },
            { 3, "sale" },
            { 4, "seo" },
            { 5, "credit_user" },
            { 6, "register_user" },
            { 7, "visitor" },
        };
        private static IStringEncryptor Encryptor
        {
            get { return DependencyResolver.Current.GetService<IStringEncryptor>(); }
        }

        public static string Encrypt(string url, string username)
        {
            string data = url + "::" + username;
            string token = Encryptor.Encrypt(data);

            return Absolute("/m/" + token);
        }

        public static string Encrypt(string url)
        {
            string data = url;
            string token = Encryptor.Encrypt(data);

            return Absolute("/n/" + token);
        }
        public static string Decrypt(string token)
        {
            string data = Encryptor.Decrypt(token);
            string[] segments = data.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            string url = segments[0];
            if (segments.Length > 1 && !string.IsNullOrEmpty(segments[1]))
            {
                //string username = segments[1];
                //LoginPersister.SignIn(username);
            }
            return url;
        }
        public static string Absolute(string url)
        {
            if (url.StartsWith("/"))
                url = url.Substring(1);

            return Root + url;
        }
        public static string Root
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return ConfigurationManager.AppSettings["DomainRoot"];
                }
                var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                var domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
              
                return domain + urlHelper.Content("~");
            }
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static bool SendEmail(string from, string to, string subject, string body)
        {
            
            try
            {
                var section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var message = new MailMessage("donotreply@example.com", to, subject, body) { IsBodyHtml = true };
                message.From = new MailAddress("donotreply@example.com", "", Encoding.UTF8);// Settings.GetSetting("Report", "CompanyName", typeof(string))
                message.ReplyToList.Add(from);
                var emailBcc = ConfigurationManager.AppSettings["EmailBCC"];
                if (!string.IsNullOrEmpty(emailBcc))
                {
                    string[] toEmail = emailBcc.Contains(";") ? emailBcc.Split(';') : emailBcc.Split(',');
                    foreach (var bcc in toEmail)
                    {
                        if (bcc != null)
                        {
                            message.Bcc.Add(bcc);
                        }

                    }
                }
            

                var client = new SmtpClient();
                client.Port = section.Network.Port;//Settings.GetSetting("EmailConfig", "Port", typeof(int));
                client.Host = section.Network.Host;//Settings.GetSetting("EmailConfig", "Host", typeof(string));
                // client.EnableSsl = section.Network.EnableSsl;//true;
                //setup up the host, increase the timeout to 60 minutes
                client.Timeout = (60 * 60 * 1000);
                client.DeliveryMethod = section.DeliveryMethod;//SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);

                client.Send(message);

            }
            catch (Exception e)
            {
                //   Tools.Logger(e.ToString(),"sendmailerr","SendMail");
                return false;
            }
            return true;
        }
    }
  
}