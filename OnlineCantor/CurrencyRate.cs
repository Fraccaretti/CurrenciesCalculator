using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    class CurrencyRate
    {
        [JsonProperty("mid")]
        public decimal Rate { get; set; }
        [JsonProperty("effectiveDate")]
        public DateTime Date { get; set; }

        public CurrencyRate(decimal rate, DateTime date)
        {
            this.Rate = rate;
            this.Date = date;
        }
    }
}
