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
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private ApplicationConfiguration _applicationConfiguration;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private SearchService _service;

        public SearchController(ILogger<SearchController> logger, ApplicationConfiguration applicationConfiguration)
        {
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
            InitialiseSearch();
        }

        private void InitialiseSearch()
        {
            _node = new Uri(_applicationConfiguration.SearchConfiguration.Uri);
            _settings = new ConnectionSettings(_node);
            _settings.ThrowExceptions(alwaysThrow: true);
            _settings.DisableDirectStreaming();
            _settings.PrettyJson();
            
            _client = new ElasticClient(_settings);
            _service = new SearchService(_client, null);
        }

        [HttpGet]
        public ActionResult<SearchServiceResults> Get(string query)
        {
            if (string.IsNullOrEmpty(query)) return null;
            
            var results = _service.Search(new CommonSearchRequest()
            {
                Query = query,
                PageSize = 10
            });
            
            return Ok(results);
        }

        [HttpGet("getactors")]
        public ActionResult<SearchServiceResults> GetActors(string query)
        {
            if (string.IsNullOrEmpty(query)) query = "*";
            
            var results = _service.GetActors(new CommonSearchRequest()
            {
                Query = query,
                PageSize = 10
            });
            
            return Ok(results);
            
            
        }
    }
}