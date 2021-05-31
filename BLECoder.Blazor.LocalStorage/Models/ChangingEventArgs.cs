namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Changing Event arguments
    /// </summary>
    public class ChangingEventArgs : ChangedEventArgs
    {
        /// <summary>
        /// Whether to cancel the event
        /// </summary>
        public bool Cancel { get; set; }
    }
}
