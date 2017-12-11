namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Controllers.ErrorsController
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;

    [Route("errors")]
    public class ErrorsController : Controller
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostError([FromBody] JObject error)
        {
            _logger.LogError(new EventId(1, "FrontEnd"), 
                $@"FE:{error["message"]}.
Headers: {string.Join("; ", Request.Headers.Select(x => x.Key + ": " + string.Join(", ", x.Value)))}.
{error}");
            return Ok();
        }
    }
}