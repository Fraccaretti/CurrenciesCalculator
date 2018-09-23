using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    public class ExchangeResult
    {
        public string PresentExchangedAmount { get; set; }
        public string MaxExchangedAmount { get; set; }
        public string MaxExchangedDate { get; set; }

        public ExchangeResult(string presentExchangedAmount, string maxExchangedAmount, string maxExchangedDate)
        {
            PresentExchangedAmount = presentExchangedAmount;
            MaxExchangedAmount = maxExchangedAmount;
            MaxExchangedDate = maxExchangedDate;
        }
    }
}
