using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies_Calculator.Models
{
    public class IndexViewModel
    {
        public List<SelectListItem> CurrenciesCodes { get; set; }

        public IndexViewModel(List<SelectListItem> items)
        {
            CurrenciesCodes = items;
        }
    }
}
