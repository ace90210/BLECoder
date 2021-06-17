using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RadzenTemplate.Server.Services;
using RadzenTemplate.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadzenTemplate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserConfigurationController : ControllerBase
    {
        private readonly ILogger<UserConfigurationController> _logger;
        private readonly UserConfigurationService userConfigurationService;

        public UserConfigurationController(ILogger<UserConfigurationController> logger, UserConfigurationService userConfigurationService)
        {
            _logger = logger;
            this.userConfigurationService = userConfigurationService;
        }

        [HttpGet("{key}")]
        public ActionResult<UserConfigurationDto> Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest();

            var config = userConfigurationService.GetConfigurationAsync(key);

            if (config == null)
                return NotFound();

            return Ok(config);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserConfigurationDto>> GetAll()
        {
            return Ok(userConfigurationService.GetUserConfigurations());
        }

        [HttpPost]
        public async Task<ActionResult> CreateConfiguration(UserConfigurationDto configuration)
        {
            var invalidResponse = CheckRequest(configuration);

            if (invalidResponse != null)
                return invalidResponse;

            var saved = await userConfigurationService.CreateConfigurationAsync(configuration);

            if (saved)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{key}")]
        public async Task<ActionResult> UpdateConfiguration(string key, [FromBody]UserConfigurationDto configuration)
        {
            var invalidResponse = CheckRequest(key, configuration);

            if (invalidResponse != null)
                return invalidResponse;

            var saved = await userConfigurationService.UpdateConfigurationAsync(configuration);

            if (saved)
                return Ok();

            return BadRequest();
        }

        private ActionResult CheckRequest(string key, UserConfigurationDto configuration)
        {
            if (configuration == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(key) || !configuration.UserUniqueIdentifier.Equals(key))
                return BadRequest("Invalid or miss matched key");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return null;
        }

        private ActionResult CheckRequest(UserConfigurationDto configuration)
        {
            if (configuration == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return null;
        }
    }
}
