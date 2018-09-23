using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    public class ExchangeParams
    {
        public string FirstCurrencyCode { get; set; }
        public string SecondCurrencyCode { get; set; }
        public decimal Amount { get; set; }

        public ExchangeParams(string firstCode, string secondCode, decimal amount)
        {
            FirstCurrencyCode = firstCode;
            SecondCurrencyCode = secondCode;
            Amount = amount;
        }
    }
}
