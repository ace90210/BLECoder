using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenTemplate.EntityFrameworkCore.SqlServer;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts;
using RadzenTemplate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RadzenTemplate.Server.Repositories
{
    public class RadzenTemplateRepository : DbContextRepository<RadzenTemplateContext>
    {
        public RadzenTemplateRepository(ILogger<RadzenTemplateRepository> logger, RadzenTemplateContext context, IMapper mapper) : base(logger, context, mapper)
        {
        }

        public List<UserConfigurationDto> GetUserConfigurations()
        {
            var configs = _context.UserConfigurations.ToList();
            return _mapper.Map<List<UserConfigurationDto>>(configs);
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

            return await SafeSaveChanges();
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
