using Microsoft.EntityFrameworkCore;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Models;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts
{
    public class RadzenTemplateContext : DbContext
    {
        public RadzenTemplateContext(DbContextOptions<RadzenTemplateContext> options) : base (options) { }

        public DbSet<JsonBlob> JsonBlobs { get; set; }

        public DbSet<UserConfiguration> UserConfigurations { get; set; }
    }
}
