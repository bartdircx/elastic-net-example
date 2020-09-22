using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Example.Search.WebApi.Configuration;
using Elastic.Example.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;

namespace Elastic.Example.Search.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private ApplicationConfiguration _applicationConfiguration;

        public SearchController(ILogger<SearchController> logger, ApplicationConfiguration applicationConfiguration)
        {
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }

        [HttpGet]
        public ActionResult<SearchServiceResults> Get(string query)
        {
            if (string.IsNullOrEmpty(query)) return null;
            
            var node = new Uri(_applicationConfiguration.SearchConfiguration.Uri);
            var settings = new ConnectionSettings(node);
            settings.ThrowExceptions(alwaysThrow: true);
            settings.DisableDirectStreaming();
            settings.PrettyJson();

            var client = new ElasticClient(settings);

            var service = new SearchService(client, null);
            var results = service.Search(new CommonSearchRequest()
            {
                Query = query,
                PageSize = 10
            });
            
            return Ok(results);
        }
    }
}