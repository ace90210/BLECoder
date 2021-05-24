using Microsoft.JSInterop;
using System;
using System.Text.Json;

namespace BLECoder.Blazor.LocalStorage
{
    public class LocalStorageServiceSync : BaseLocalStorageService, ILocalStorageServiceSync
    {
        private readonly IJSInProcessRuntime _jsInProcessRuntime;

        public LocalStorageServiceSync(IJSInProcessRuntime jsInProcessRuntime, JsonSerializerOptions options) : base(options)
        {
            _jsInProcessRuntime = jsInProcessRuntime;
        }
                
        public T GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedValue = _jsInProcessRuntime.Invoke<string>("localStorage.getItem", key);

            if (!string.IsNullOrWhiteSpace(serialisedValue))
                return JsonSerializer.Deserialize<T>(serialisedValue, _jsonOptions);

            return default(T);
        }

        public void SetItem(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = RaiseOnChangingSync(key, value);

            if (e.Cancel)
                return;

            _jsInProcessRuntime.Invoke<object>("localStorage.setItem", key, JsonSerializer.Serialize(value, _jsonOptions));

            RaiseOnChanged(key, e.OldValue, value);
        }
        public void RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
                
            _jsInProcessRuntime.Invoke<object>("localStorage.removeItem", key);
        }

        public void Clear()
        {
            _jsInProcessRuntime.Invoke<object>("localStorage.clear");
        }

        public int Length()
        {
            return _jsInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        }

        public string Key(int index)
        {
            return _jsInProcessRuntime.Invoke<string>("localStorage.key", index);
        }

        public bool ContainKey(string key)
        {
            return _jsInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
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
