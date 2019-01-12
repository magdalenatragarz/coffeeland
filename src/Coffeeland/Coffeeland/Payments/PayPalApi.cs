﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace Coffeeland.Payments
{
    public static class PayPalApi
    {
        // sample payment id - "PAY-2RR93057JR3600055LQ5FWMA"
        public static string GetTotalAmount(string paymentId)
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            apiContext.Config = ConfigManager.Instance.GetProperties();
            apiContext.Config["connectionTimeout"] = "100000";
            var payment = Payment.Get(apiContext, paymentId);

            return payment.transactions[0].amount.total;
        }
        
    }
}