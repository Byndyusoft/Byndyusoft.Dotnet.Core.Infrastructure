namespace Web.Application.Controllers.ValuesController
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("values")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        /// <summary>
        /// Provides values array
        /// </summary>
        /// <returns>Configuration values</returns>
        [HttpGet]
        public async Task<string[]> Get()
        {
            _logger.LogInformation("Get values request");
            return new[] {"test"};
        }
    }
}