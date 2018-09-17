using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Currencies_Calculator.Models
{
    public class Currency
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name_plural")]
        public string NamePlural { get; set; }

        public Currency(string symbol, string name, string code, string namePlural)
        {
            this.Symbol = symbol;
            this.Name = name;
            this.Code = code;
            this.NamePlural = namePlural;
        }
    }
}
