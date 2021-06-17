using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Models;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts
{
    public class RadzenTemplateContext : BaseDbContext
    {
        public RadzenTemplateContext(DbContextOptions<RadzenTemplateContext> options, IMapper mapper, ILogger<RadzenTemplateContext> logger) : base (options, mapper, logger) { }

        public DbSet<JsonBlob> JsonBlobs { get; set; }

        public DbSet<UserConfiguration> UserConfigurations { get; set; }
    }
}
