using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    internal class NBPClient : RestClient
    {
        public NBPClient() : base (CantorConfig.BASE_API_URL)
        {
                
        }

        public List<CurrencyRate> GetRates(string currencyCode)
        {
            var request = CreateRequest(currencyCode);
            IRestResponse response = Execute(request);

            var deserializer = new JsonDeserializer();
            List<CurrencyRate> rates = deserializer.DeserializeToCurrencyRates(response.Content);
    
            return rates;
        }

        private RestRequest CreateRequest(string currencyCode)
        {
            string todayDate = DateTime.Now.ToString(CantorConfig.DATE_FORMAT);
            string monthAgoDate = DateTime.Now.AddDays(-30).ToString(CantorConfig.DATE_FORMAT);

            string requestURL = currencyCode + "/" + monthAgoDate + "/" + todayDate + CantorConfig.RESPONSE_FORMAT;
            var request = new RestRequest(requestURL, Method.GET);

            return request;
        }
    }
}
