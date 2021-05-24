using System;
using System.Text.Json;

namespace BLECoder.Blazor.LocalStorage
{
    public class BaseLocalStorageService 
    {
        protected JsonSerializerOptions _jsonOptions;

        public BaseLocalStorageService(JsonSerializerOptions options)
        {
            _jsonOptions = options ?? throw new ArgumentNullException("JsonSerializerOptions not registered");

        }
        
        public event EventHandler<ChangedEventArgs> Changed;

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
