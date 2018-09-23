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
using OnlineCantor;

namespace Currencies_Calculator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var currency in InMemoryDataBase.CurrenciesList)
            {
                items.Add(new SelectListItem { Text = currency, Value = currency});
            }

            ViewBag.FirstCurrency = items;
            ViewBag.SecondCurrency = items;

            return View();
        }

        [HttpPost]
        public ActionResult<string[]> Exchange(string firstCurrency, string secondCurrency, float amount)
        {
            Cantor cantor = new Cantor();
            var result = cantor.Exchange(firstCurrency, secondCurrency, amount);
            if (!result.Any())
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
