﻿using System.Threading.Tasks;

namespace BLECoder.Blazor.LocalStorage
{
    public interface ILocalStorageServiceAsync : IBaseLocalStorageService
    {
        Task ClearAsync();

        Task<T> GetItemAsync<T>(string key);

        Task<string> KeyAsync(int index);

        /// <summary>
        /// Checks if the key exists in local storage but does not check the value.
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        Task<bool> ContainKeyAsync(string key);

        Task<int> LengthAsync();

        Task RemoveItemAsync(string key);

        Task SetItemAsync(string key, object data);
    }
}
