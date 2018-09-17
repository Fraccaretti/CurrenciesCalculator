using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Currencies_Calculator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Currencies_Calculator.Content;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Currencies_Calculator.Controllers
{
    public class HomeController : Controller
    {

        private readonly string DATE_FORMAT = "yyyy-MM-dd";
        private readonly string RESPONSE_FORMAT = "?format=json";
        private readonly string baseURL = "http://api.nbp.pl/api/exchangerates/rates/a/";

        public IActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var currency in InMemoryDataBase.CurrenciesList)
            {
                items.Add(new SelectListItem { Text = currency.Code, Value = currency.Code });
            }

            ViewBag.FirstCurrency = items;
            ViewBag.SecondCurrency = items;

            return View();
        }

        [HttpGet]
        public string[] Exchange(string firstCurrency, string secondCurrency, float amount)
        {
            Dictionary<DateTime, float> firstCurrencyRates = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> secondCurrencyRates = new Dictionary<DateTime, float>();

            if (firstCurrency != "PLN")
            {
                firstCurrencyRates = GetCurrencyExchangeRates(firstCurrency);
                if(secondCurrency != "PLN")
                {
                    secondCurrencyRates = GetCurrencyExchangeRates(secondCurrency);
                } else
                {
                    foreach (var key in firstCurrencyRates.Keys)
                    {
                        secondCurrencyRates.Add(key, 1);
                    }
                }
            } else
            {
                secondCurrencyRates = GetCurrencyExchangeRates(secondCurrency);
                foreach (var key in secondCurrencyRates.Keys)
                {
                    firstCurrencyRates.Add(key, 1);
                }
            }

            //for both rates dictionaries keys should be same (from API)
            List<DateTime> ratesDictionariesKeys = new List<DateTime>(firstCurrencyRates.Keys);

            Dictionary<DateTime, float> exchangedAmounts = new Dictionary<DateTime, float>();

            float maxExchangedAmount = 0;
            string maxExchangedDate = "";

            float presentExchangedAmount = 0;

            foreach (var key in ratesDictionariesKeys)
            {
                float exchangedAmount = (amount * firstCurrencyRates[key]) / secondCurrencyRates[key];

                if (exchangedAmount > maxExchangedAmount)
                {
                    maxExchangedAmount = exchangedAmount;
                    maxExchangedDate = key.ToString(DATE_FORMAT);
                }

                if (key == ratesDictionariesKeys.Last())
                {
                    presentExchangedAmount = exchangedAmount;
                }
            }

            // round to 2 decimal places
            string[] stringArray = { presentExchangedAmount.ToString("n2"), maxExchangedAmount.ToString("n2"), maxExchangedDate };
            return stringArray;
        }

        private Dictionary<DateTime, float> GetCurrencyExchangeRates(string currencyCode)
        {
            // get the time interval
            string todayDate = DateTime.Now.ToString(DATE_FORMAT);
            string monthAgoDate = DateTime.Now.AddDays(-30).ToString(DATE_FORMAT);

            //setup REST client and request
            RestClient client = new RestClient(baseURL);
            string requestURL = currencyCode + "/" + monthAgoDate + "/" + todayDate + RESPONSE_FORMAT;
            var request = new RestRequest(requestURL, Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);

            // deserialize data into dictionary with DateTime key and rate (float) value
            var jsonString = response.Content;
            dynamic jsonObject = JObject.Parse(jsonString);
            JArray jsonRates = jsonObject["rates"];
            Dictionary<DateTime, float> rates = new Dictionary<DateTime, float>();
            foreach (JObject rate in jsonRates.Children())
            {
                DateTime date = DateTime.ParseExact((string)rate["effectiveDate"], DATE_FORMAT,
                                       System.Globalization.CultureInfo.InvariantCulture);
                rates.Add(date, (float)rate["mid"]);
            }
            
            return rates;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult SelectCategory()
        {
            return View();
        }
    }
}
