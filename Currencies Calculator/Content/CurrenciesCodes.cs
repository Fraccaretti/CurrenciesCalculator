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
    public class CurrenciesCodes
    {
        public static List<string> DeserializeFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Content\Currencies.json");
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();

            using (StreamReader file = File.OpenText(path))
            {
                string jsonString = file.ReadToEnd();
                dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
            }

            return dict.Keys.ToList<string>();
        }
    }
}
