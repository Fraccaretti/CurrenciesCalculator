using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Currencies_Calculator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;
using Newtonsoft.Json.Linq;
using OnlineCantor;
using System.Globalization;

namespace Currencies_Calculator.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var currency in CurrenciesCodes.DeserializeFromFile())
            {
                items.Add(new SelectListItem { Text = currency, Value = currency});
            }

            IndexViewModel model = new IndexViewModel(items);

            return View(model);
        }

        [HttpPost]
        public ActionResult<string> Exchange(string firstCurrency, string secondCurrency, string amountString)
        {
            var provider = new CultureInfo("en-US");
            decimal amount = Decimal.Parse(amountString, provider);

            CantorService cantorServiceClient = new CantorService();
            var result = cantorServiceClient.Exchange(new ExchangeParams(firstCurrency, secondCurrency, amount));
            if (result == null)
            {
                return NotFound();
            }
            return result;
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
