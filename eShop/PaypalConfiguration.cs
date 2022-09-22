using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eShop
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AYgnfRtmL8oBho2XZetPjwGSqTBqwcZxcIWT0OYLJK_Jow";
            clientSecret = "EJoG_4mdDp-Qp9U_pZAfT_l4bKztmm7QE-2tPbw0aNmP3BS_o8w5Q6VfEX_hZ371IFb4txOLtYFFwZ_F";
        }

        private static Dictionary<string,string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            //string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            string accessToken = "access_token$sandbox$y4yky5f2447yc7gy$691290088fd949db7ee5811c8bbdcc33";
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = getconfig();
            return apiContext;
        }
    }
}