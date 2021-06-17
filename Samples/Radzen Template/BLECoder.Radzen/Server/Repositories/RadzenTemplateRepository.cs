using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenTemplate.EntityFrameworkCore.SqlServer;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Models;
using RadzenTemplate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RadzenTemplate.Server.Repositories
{
    public class RadzenTemplateRepository
    {
        private readonly ILogger<RadzenTemplateRepository> _logger;
        private readonly RadzenTemplateContext _context;
        private readonly IMapper _mapper;

        public RadzenTemplateRepository(ILogger<RadzenTemplateRepository> logger, RadzenTemplateContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }



        public async Task<bool> CreateUserConfigurationAsync(UserConfigurationDto config)
        {
            return await _context.CreateAsync<UserConfigurationDto, UserConfiguration>(config.UserUniqueIdentifier, config);
        }

        public async Task<UserConfigurationDto> GetUserConfigurationAsync(string key)
        {
            return await _context.GetAsync<UserConfigurationDto, UserConfiguration>(key);
        }

        public async Task<bool> UpdateUserConfigurationAsync(UserConfigurationDto config)
        {
            return await _context.UpdateAsync<UserConfigurationDto, UserConfiguration>(config.UserUniqueIdentifier, config);
        }

        public List<UserConfigurationDto> GetUserConfigurations()
        {
            return _context.GetList<UserConfigurationDto, UserConfiguration>().ToList();
        }

        public UserConfigurationDto GetUserConfiguration(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key not provided");

            var foundConfig = _context.UserConfigurations.FirstOrDefault(uc => uc.UserUniqueIdentifier.Equals(key));

            if (foundConfig == null)
                return null;
            
            return _mapper.Map<UserConfigurationDto>(foundConfig);
        }

        public async Task<bool> SetBlob(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
                return false;

            var blob = new JsonBlob() { Key = key };
            try
            {
                blob.Data = JsonSerializer.Serialize(value);
            }
            catch (Exception)
            {
                return false;
            }
            
            var entity = _context.JsonBlobs.Find(key);


            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(blob);
            }
            else
            {
                await _context.JsonBlobs.AddAsync(blob);
            }

            return await _context.SafeSaveChanges();
        }

        public T GetBlob<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default;

            var blob = _context.JsonBlobs.FirstOrDefault(jb => jb.Key.Equals(key));
            
            try
            {
                var data = JsonSerializer.Deserialize<T>(blob.Data);
                return data;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
