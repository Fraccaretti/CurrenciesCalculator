using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineCantor.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace OnlineCantor
{
    public class CantorService : ICantor
    {
        public string Exchange(ExchangeParams @params)
        {
            NBPClient nbpClient = new NBPClient();
            List<CurrencyRate> firstCurrencyRates = new List<CurrencyRate>();
            List<CurrencyRate> secondCurrencyRates = new List<CurrencyRate>();

            if (@params.FirstCurrencyCode == "PLN")
            {
                secondCurrencyRates = nbpClient.GetRates(@params.SecondCurrencyCode);
                foreach (var rateObject in secondCurrencyRates)
                {
                    firstCurrencyRates.Add(new CurrencyRate(1, rateObject.Date));
                }
            } else
            {
                firstCurrencyRates = nbpClient.GetRates(@params.FirstCurrencyCode);
                if (@params.SecondCurrencyCode == "PLN")
                {
                    foreach (var rateObject in firstCurrencyRates)
                    {
                        secondCurrencyRates.Add(new CurrencyRate(1, rateObject.Date));
                    }
                } else
                {
                    secondCurrencyRates = nbpClient.GetRates(@params.SecondCurrencyCode);
                }
            }

            decimal maxExchangedAmount = 0;
            string maxExchangedDate = "";

            decimal presentExchangedAmount = 0;

            foreach (var firstCurrency in firstCurrencyRates)
            {
                CurrencyRate secondCurrencyRateObject = secondCurrencyRates.FirstOrDefault(o => o.Date == firstCurrency.Date);
                decimal exchangedAmount = (@params.Amount * firstCurrency.Rate) / secondCurrencyRateObject.Rate;

                if (exchangedAmount > maxExchangedAmount)
                {
                    maxExchangedAmount = exchangedAmount;
                    maxExchangedDate = firstCurrency.Date.ToString(CantorConfig.DATE_FORMAT);
                }

                if (firstCurrencyRates.Last() == firstCurrency)
                {
                    presentExchangedAmount = exchangedAmount;
                }
            }

            // round to 2 decimal places
            ExchangeResult result = new ExchangeResult(presentExchangedAmount.ToString("n2"), maxExchangedAmount.ToString("n2"), maxExchangedDate);
            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }
    }
}
