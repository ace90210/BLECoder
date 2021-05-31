using Microsoft.JSInterop;
using System;
using System.Text.Json;

namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Local storage access using syncronous code.
    /// </summary>
    public class LocalStorageServiceSync : BaseLocalStorageService, ILocalStorageServiceSync
    {
        private readonly IJSInProcessRuntime _jsInProcessRuntime;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="jSRuntime">The injected JS Process Runtime</param>
        /// <param name="options">The JSON Serialisation options</param>
        public LocalStorageServiceSync(IJSInProcessRuntime jsInProcessRuntime, JsonSerializerOptions options) : base(options)
        {
            _jsInProcessRuntime = jsInProcessRuntime;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _jsInProcessRuntime.Invoke<object>("localStorage.clear");
        }

        /// <inheritdoc/>
        public bool ContainKey(string key)
        {
            return _jsInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
        }
        /// <inheritdoc/>
        public string Key(int index)
        {
            return _jsInProcessRuntime.Invoke<string>("localStorage.key", index);
        }

        /// <inheritdoc/>
        public int Length()
        {
            return _jsInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        }

        /// <inheritdoc/>        
        public T GetItem<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedValue = _jsInProcessRuntime.Invoke<string>("localStorage.getItem", key);

            if (!string.IsNullOrWhiteSpace(serialisedValue))
                return JsonSerializer.Deserialize<T>(serialisedValue, _jsonOptions);

            return defaultValue;
        }

        /// <inheritdoc/>
        public bool SetItem(string key, object value, bool overwriteExisting = true)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!overwriteExisting)
            {
                var exists = ContainKey(key);

                if (exists)
                {
                    return false;
                }
            }

            var e = RaiseOnChangingSync(key, value);

            if (e.Cancel)
                return false;

            _jsInProcessRuntime.Invoke<object>("localStorage.setItem", key, JsonSerializer.Serialize(value, _jsonOptions));

            RaiseOnChanged(key, e.OldValue, value);

            return true;
        }

        /// <inheritdoc/>
        public void RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
                
            _jsInProcessRuntime.Invoke<object>("localStorage.removeItem", key);
        }

        public event EventHandler<ChangingEventArgs> Changing;

        private ChangingEventArgs RaiseOnChangingSync(string key, object value)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = GetItem<object>(key),
                NewValue = value
            };

            Changing?.Invoke(this, e);

            return e;
        }
    }
}
