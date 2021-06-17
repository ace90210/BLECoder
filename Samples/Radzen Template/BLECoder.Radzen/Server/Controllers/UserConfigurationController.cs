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
            var config = userConfigurationService.GetConfiguration(key);

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
            var saved = await userConfigurationService.CreateConfiguration(configuration);

            if (saved)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{key}")]
        public async Task<ActionResult> UpdateConfiguration(string key, [FromBody]UserConfigurationDto configuration)
        {
            if (string.IsNullOrWhiteSpace(key) || !configuration.UserUniqueIdentifier.Equals(key))
                return BadRequest("Invalid or miss matched key");

            var saved = await userConfigurationService.UpdateConfigurationAsync(key, configuration);

            if (saved)
                return Ok();

            return BadRequest();
        }
    }
}
