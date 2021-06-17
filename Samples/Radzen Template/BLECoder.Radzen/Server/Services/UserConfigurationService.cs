using RadzenTemplate.EntityFrameworkCore.SqlServer.Models;
using RadzenTemplate.Server.Repositories;
using RadzenTemplate.Shared;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> CreateConfiguration(UserConfigurationDto config)
        {
            return await repository.CreateAsync<UserConfigurationDto, UserConfiguration>(config.UserUniqueIdentifier, config);
        }

        public async Task<UserConfigurationDto> GetConfiguration(string key)
        {
            return await repository.GetAsync<UserConfigurationDto, UserConfiguration>(key);
        }

        public async Task<bool> UpdateConfigurationAsync(string key, UserConfigurationDto config)
        {
            return await repository.UpdateAsync<UserConfigurationDto, UserConfiguration>(key, config);
        }

        public List<UserConfigurationDto> GetUserConfigurations()
        {
            return repository.GetList<UserConfigurationDto, UserConfiguration>().ToList();
        }
    }
}
