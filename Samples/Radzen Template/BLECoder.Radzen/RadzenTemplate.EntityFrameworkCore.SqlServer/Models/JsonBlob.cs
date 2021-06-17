using System.ComponentModel.DataAnnotations;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer
{
    public class JsonBlob
    {
        [Key]
        [MaxLength(32)]
        public string Key { get; set; }

        public string Data { get; set; }
    }
}
