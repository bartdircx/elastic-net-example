using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Elastic.Example.Search.WebApi.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Elastic.Example.Search.WebApi.Configuration
{
    public class ApplicationConfiguration
    {
        public SearchConfiguration SearchConfiguration { get; set; }
        public ApplicationConfiguration()
        {
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("ConfigBuilder created");

            configRoot.GetSection("SearchApp").Bind(this);

            Console.WriteLine("ConfigBuilder binded");

        }
    }

    public class SearchConfiguration
    {
        public string Uri { get; set; }
    }
}