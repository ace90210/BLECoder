using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPatch("{key}")]
        public async Task<ActionResult> PatchConfiguration(string key, [FromBody] JsonPatchDocument<UserConfigurationDto> patchDocument)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("Invalid or miss matched key");

            if (patchDocument == null)
                return BadRequest("Invalid or miss patch document");

            var response = await userConfigurationService.GetConfiguration(key);

            patchDocument.ApplyTo(response);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userConfigurationService.UpdateConfigurationAsync(key, response);

            return Ok(result);
        }
    }
}
