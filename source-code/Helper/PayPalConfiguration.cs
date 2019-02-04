using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using PayPal.Api;

namespace WebApplication1.Helper
{
    public class PayPalConfiguration
    {
        veebdbEntities db =new veebdbEntities();
        public string ClientId;
        public string ClientSecret;
        private string host = "";
        // Static constructor for setting the readonly static members.
        public PayPalConfiguration()
        {

            var config = GetConfig();
            //ClientId = config["clientId"];
            //ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public Dictionary<string, string> GetConfig()
        {
            var mode = db.settings.FirstOrDefault(x => x.typeId == "paypal" && x.code == "mode");
            var connectionTimeout = db.settings.FirstOrDefault(x => x.typeId == "paypal" && x.code == "connectionTimeout");
            var requestRetries = db.settings.FirstOrDefault(x => x.typeId == "paypal" && x.code == "requestRetries");
            var clientId = db.settings.FirstOrDefault(x => x.typeId == "paypal" && x.code == "clientId");
            var clientSecret = db.settings.FirstOrDefault(x => x.typeId == "paypal" && x.code == "clientSecret");
            Dictionary <string, string> dic = new Dictionary<string, string>();
            dic.Add("mode", mode != null ? mode.link : WebConfigurationManager.AppSettings["mode"]);
            dic.Add("connectionTimeout", connectionTimeout != null ? connectionTimeout.link : WebConfigurationManager.AppSettings["connectionTimeout"]);
            dic.Add("requestRetries", requestRetries != null ? requestRetries.link : WebConfigurationManager.AppSettings["requestRetries"]);
            dic.Add("clientId", clientId != null ? clientId.link : WebConfigurationManager.AppSettings["clientId"]);
            dic.Add("clientSecret", clientSecret != null ? clientSecret.link : WebConfigurationManager.AppSettings["clientSecret"]);

            ClientId = clientId != null ? clientId.link : WebConfigurationManager.AppSettings["clientId"];
            ClientSecret = clientSecret != null ? clientSecret.link : WebConfigurationManager.AppSettings["clientSecret"];
         
            return dic;
        }

        // Create accessToken
        private string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public APIContext GetAPIContext(string accessToken = "", string requestID = "")
        {
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken, string.IsNullOrEmpty(requestID) ? Guid.NewGuid().ToString() : requestID);
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}