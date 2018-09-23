using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace OnlineCantor
{
    internal class CantorConfig
    {
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly string RESPONSE_FORMAT = "?format=json";
        public static readonly string BASE_API_URL = "http://api.nbp.pl/api/exchangerates/rates/a/";
    }

    internal class RateObject
    {
        public float Rate { get; set; }
        public DateTime Date { get; set; }

        public RateObject(float rate, DateTime date)
        {
            this.Rate = rate;
            this.Date = date;
        }
    }

    public class Cantor
    {
        private RestClient restClient = new RestClient(CantorConfig.BASE_API_URL);

        public string[] Exchange(string firstCurrency, string secondCurrency, float amount)
        {
            List<RateObject> firstCurrencyRates = new List<RateObject>();
            List<RateObject> secondCurrencyRates = new List<RateObject>();

            if (firstCurrency == "PLN")
            {
                secondCurrencyRates = GetRates(secondCurrency);
                foreach (var rateObject in secondCurrencyRates)
                {
                    firstCurrencyRates.Add(new RateObject(1, rateObject.Date));
                }
            } else
            {
                firstCurrencyRates = GetRates(firstCurrency);
                if (secondCurrency == "PLN")
                {
                    foreach (var rateObject in firstCurrencyRates)
                    {
                        secondCurrencyRates.Add(new RateObject(1, rateObject.Date));
                    }
                } else
                {
                    secondCurrencyRates = GetRates(secondCurrency);
                }
            }

            float maxExchangedAmount = 0;
            string maxExchangedDate = "";

            float presentExchangedAmount = 0;

            foreach (var firstCurrencyRateObject in firstCurrencyRates)
            {
                RateObject secondCurrencyRateObject = secondCurrencyRates.Where(o => o.Date == firstCurrencyRateObject.Date).Single();
                float exchangedAmount = (amount * firstCurrencyRateObject.Rate) / secondCurrencyRateObject.Rate;

                if (exchangedAmount > maxExchangedAmount)
                {
                    maxExchangedAmount = exchangedAmount;
                    maxExchangedDate = firstCurrencyRateObject.Date.ToString(CantorConfig.DATE_FORMAT);
                }

                if (firstCurrencyRates.Last() == firstCurrencyRateObject)
                {
                    presentExchangedAmount = exchangedAmount;
                }
            }

            // round to 2 decimal places
            string[] stringArray = { presentExchangedAmount.ToString("n2"), maxExchangedAmount.ToString("n2"), maxExchangedDate };
            return stringArray;
        }

        private List<RateObject> GetRates(string currencyCode)
        {
            var request = CreateRequest(currencyCode);
            IRestResponse response = restClient.Execute(request);
            List<RateObject> rates = DeserializeJSON(response.Content);
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

        /// <summary>
        /// Deserialize json data into RateObjects (contains rate and date).
        /// </summary>
        /// <returns></returns>     
        private List<RateObject> DeserializeJSON(string jsonString)
        {            
            dynamic jsonObject = JObject.Parse(jsonString);
            JArray jsonRates = jsonObject["rates"];

            List<RateObject> rateObjects = new List<RateObject>();
            foreach (JObject jsonRate in jsonRates.Children())
            {
                float rate = (float)jsonRate["mid"];
                DateTime date = DateTime.ParseExact((string)jsonRate["effectiveDate"], CantorConfig.DATE_FORMAT,
                                       System.Globalization.CultureInfo.InvariantCulture);
                RateObject rateObject = new RateObject(rate, date);
                rateObjects.Add(rateObject);
            }

            return rateObjects;
        }
    }
}
