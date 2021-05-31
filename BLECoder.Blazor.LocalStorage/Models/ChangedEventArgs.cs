namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Changed event arguments
    /// </summary>
    public class ChangedEventArgs
    {
        /// <summary>
        /// The Storage Key that was changed
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The old value
        /// </summary>
        public object OldValue { get; set; }
        
        /// <summary>
        /// The new value
        /// </summary>
        public object NewValue { get; set; }
    }
}