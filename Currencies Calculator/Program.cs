using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Currencies_Calculator.Content;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Currencies_Calculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PopulateInMemoryDataBase();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static void PopulateInMemoryDataBase()
        {
            InMemoryDataBase.CurrenciesList = CurrenciesSerializer.DeserializeFromFile();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
