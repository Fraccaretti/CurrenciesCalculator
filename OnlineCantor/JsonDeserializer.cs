using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    class JsonDeserializer
    {
        /// <summary>
        /// Deserialize json data into CurrencyRate (contains rate and date).
        /// </summary>
        /// <returns></returns>     
        public List<CurrencyRate> DeserializeToCurrencyRates(string jsonString)
        {
            JObject obj = JObject.Parse(jsonString);
            JArray jsonRates = (JArray)obj["rates"];
            List<CurrencyRate> rateObjects = jsonRates.ToObject<List<CurrencyRate>>();

            return rateObjects;
        }
    }
}
