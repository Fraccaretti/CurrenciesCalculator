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
        public static List<string> DeserializeFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Content\Currencies.json");
            List<string> items = new List<string>();

            using (StreamReader file = File.OpenText(path))
            {
                string jsonString = file.ReadToEnd();

                Dictionary<string, Dictionary<string, string>> dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);

                foreach (string jsonCurrency in dict.Keys)
                {

                    items.Add(jsonCurrency);
                }
            }

            return items;
        }
    }
}
