using RadzenTemplate.Server.Repositories;
using RadzenTemplate.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadzenTemplate.Server.Services
{
    public class UserConfigurationService
    {
        private readonly RadzenTemplateRepository repository;

        public UserConfigurationService(RadzenTemplateRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CreateConfigurationAsync(UserConfigurationDto config)
        {
            return await repository.CreateUserConfigurationAsync(config);
        }

        public async Task<UserConfigurationDto> GetConfigurationAsync(string key)
        {
            return await repository.GetUserConfigurationAsync(key);
        }

        public async Task<bool> UpdateConfigurationAsync(UserConfigurationDto config)
        {
            return await repository.UpdateUserConfigurationAsync(config);
        }

        public List<UserConfigurationDto> GetUserConfigurations()
        {
            return repository.GetUserConfigurations();
        }
    }
}
