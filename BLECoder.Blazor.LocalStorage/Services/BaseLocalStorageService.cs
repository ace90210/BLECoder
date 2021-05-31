using System;
using System.Text.Json;

namespace BLECoder.Blazor.LocalStorage
{
    /// <inheritdoc/>
    public class BaseLocalStorageService 
    {
        protected JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="options">JSON Serializer options to use.</param>
        public BaseLocalStorageService(JsonSerializerOptions options)
        {
            _jsonOptions = options ?? throw new ArgumentNullException("JsonSerializerOptions not registered");
        }

        /// <inheritdoc/>
        public event  EventHandler<ChangedEventArgs> Changed;

        /// <summary>
        /// Raises the on changed event
        /// </summary>
        /// <param name="key">The Storage Key Changed</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="data">The new value</param>
        protected void RaiseOnChanged(string key, object oldValue, object data)
        {
            var e = new ChangedEventArgs
            {
                Key = key,
                OldValue = oldValue,
                NewValue = data
            };

            Changed?.Invoke(this, e);
        }
    }
}
