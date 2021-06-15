using Microsoft.EntityFrameworkCore;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts
{
    public class RadzenTemplateContext : DbContext
    {
        public RadzenTemplateContext(DbContextOptions<RadzenTemplateContext> options) : base (options) { }

        public DbSet<JsonBlob> JsonBlobs { get; set; }
    }
}
