namespace RadzenTemplate.Client.Models.Radzen
{
    public class ThemeItem
    {
        /// <summary>
        /// Label for dropdown
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The theme name (used in the css filename for default filenames)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// If set used to set the css file location.
        /// </summary>
        public string CustomCssPath { get; set; }
    }
}
