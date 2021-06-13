using System.Linq;

namespace RadzenTemplate.Client.Models.Radzen
{
    public class ThemeState
    {
        public ThemeItem CurrentTheme { get; set; } = SupportedThemes.SupportedThemeItems.First();
    }
}
