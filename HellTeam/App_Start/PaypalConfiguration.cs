using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChillLearn
{
    //Get Configuration from web.config File
    public class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PaypalConfiguration()
        {
            try
            {
                var config = GetConfig();
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }
            catch (Exception ex)
            {
            }
        }

        public static Dictionary<string,string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }
        //create Access Token
        private static string GetAccessToken()
        {
            string accesstoken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accesstoken;
        }

        public static APIContext GetAPIContext()
        {
            try
            {
                var apiContext = new APIContext(GetAccessToken());
                apiContext.Config = GetConfig();
                return apiContext;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}