using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCantor
{
    class CantorConfig
    {
        public static readonly string BASE_API_URL = "http://api.nbp.pl/api/exchangerates/rates/a/";
        public static readonly string RESPONSE_FORMAT = "?format=json";
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
    }
}
