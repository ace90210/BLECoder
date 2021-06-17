using System.ComponentModel.DataAnnotations;

namespace RadzenTemplate.Shared
{
    public class UserConfigurationDto
    { 
        [MaxLength(128)]
        public string UserUniqueIdentifier { get; set; }

        [MaxLength(32)]
        public string PreferredTheme { get; set; }

        public int LoginCount { get; set; }

        [MaxLength(16)]
        public string SiteNickname { get; set; }
    }
}
