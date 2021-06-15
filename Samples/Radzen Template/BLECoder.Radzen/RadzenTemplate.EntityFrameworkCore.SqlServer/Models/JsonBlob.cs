using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer
{
    public class JsonBlob
    {
        [Key]
        public string Key { get; set; }

        [Column(TypeName = "jsonb")]
        public string Data { get; set; }
    }
}
