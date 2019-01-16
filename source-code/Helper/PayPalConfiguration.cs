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
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("mode", WebConfigurationManager.AppSettings["mode"]);
            dic.Add("connectionTimeout", WebConfigurationManager.AppSettings["connectionTimeout"]);
            dic.Add("requestRetries", WebConfigurationManager.AppSettings["requestRetries"]);
            dic.Add("clientId", WebConfigurationManager.AppSettings["clientId"]);
            dic.Add("clientSecret", WebConfigurationManager.AppSettings["clientSecret"]);

            ClientId = WebConfigurationManager.AppSettings["clientId"];
            ClientSecret = WebConfigurationManager.AppSettings["clientSecret"];
            //var temp = ConfigManager.Instance.GetProperties();
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