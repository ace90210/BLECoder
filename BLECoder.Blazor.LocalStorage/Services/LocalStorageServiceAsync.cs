using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Local storage access using asyncronous code.
    /// </summary>
    public class LocalStorageServiceAsync : BaseLocalStorageService, ILocalStorageServiceAsync
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="jSRuntime">The injected JS Runtime</param>
        /// <param name="options">The JSON Serialisation options</param>
        public LocalStorageServiceAsync(IJSRuntime jSRuntime, JsonSerializerOptions options) : base(options)
        {
            _jsRuntime = jSRuntime;
        }

        /// <inheritdoc/>
        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.clear");
        }

        /// <inheritdoc/>
        public async Task<bool> ContainKeyAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);
        }

        /// <inheritdoc/>
        public async Task<T> GetItemAsync<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return defaultValue;

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        /// <inheritdoc/>
        public async Task<string> KeyAsync(int index)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.key", index);
        }

        /// <inheritdoc/>
        public async Task<int> LengthAsync()
        {
            return await _jsRuntime.InvokeAsync<int>("eval", "localStorage.length");
        }

        /// <inheritdoc/>
        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", key);
        }

        /// <inheritdoc/>
        public async Task<bool> SetItemAsync(string key, object data, bool overwriteExisting = true)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!overwriteExisting)
            {
                var exists = await ContainKeyAsync(key);

                if (exists)
                {
                    return false;
                }
            }

            var e = await RaiseOnChangingAsync(key, data);

            if (e.Cancel)
                return false;

            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", key, JsonSerializer.Serialize(data, _jsonOptions));

            RaiseOnChanged(key, e.OldValue, data);

            return true;
        }

        /// <inheritdoc/>
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
