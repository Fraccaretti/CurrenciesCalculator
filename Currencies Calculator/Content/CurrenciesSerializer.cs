using Currencies_Calculator.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Currencies_Calculator
{
    public class CurrenciesSerializer
    {
        public static List<Currency> DeserializeFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Content\Currencies.json");
            List<Currency> items = new List<Currency>();

            using (StreamReader file = File.OpenText(path))
            {
                string jsonString = file.ReadToEnd();
                Dictionary<string, Currency> dict = JsonConvert.DeserializeObject<Dictionary<string, Currency>>(jsonString);
                items = dict.Values.ToList<Currency>();
            }
            
            return items;
        }
    }
}
