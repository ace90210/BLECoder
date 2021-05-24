using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.LocalStorage
{
    public class LocalStorageServiceAsync : BaseLocalStorageService, ILocalStorageServiceAsync
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageServiceAsync(IJSRuntime jSRuntime, JsonSerializerOptions options) : base(options)
        {
            _jsRuntime = jSRuntime;
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default(T);

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        public async Task SetItemAsync(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data);

            if (e.Cancel)
                return;

            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", key, JsonSerializer.Serialize(data, _jsonOptions));

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", key);
        }

        public async Task ClearAsync() 
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.clear"); 
        }

        public async Task<int> LengthAsync()
        {
            return await _jsRuntime.InvokeAsync<int>("eval", "localStorage.length");
        }

        public async Task<string> KeyAsync(int index)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.key", index);
        }

        public async Task<bool> ContainKeyAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);
        }

        public event EventHandler<ChangingEventArgs> Changing;
        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = await GetItemAsync<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }
    }
}
