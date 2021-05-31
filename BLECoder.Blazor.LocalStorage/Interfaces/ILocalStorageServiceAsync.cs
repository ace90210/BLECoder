using System.Threading.Tasks;

namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// Local storage access using asyncronous code.
    /// </summary>
    public interface ILocalStorageServiceAsync : IBaseLocalStorageService
    {
        /// <summary>
        /// Clears All Data from local storage
        /// </summary>
        /// <returns>The Async Task</returns>
        Task ClearAsync();

        /// <summary>
        /// Checks if the key exists in local storage but does not check the value.
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        Task<bool> ContainKeyAsync(string key);

        /// <summary>
        /// Get object from storage and deserialise as T
        /// </summary>
        /// <typeparam name="T">The type to deserialise as</typeparam>
        /// <param name="key">The Key to find the item from storage</param>
        /// <param name="defaultValue">(Optional): The default value to return if not found.</param>
        /// <returns>The deserialised value if found. If not the default value of T</returns>
        Task<T> GetItemAsync<T>(string key, T defaultValue = default);

        /// <summary>
        /// Get the key on a given position.
        /// </summary>
        /// <param name="index">The index of the key</param>
        /// <returns>The Key name</returns>
        Task<string> KeyAsync(int index);

        /// <summary>
        /// Returns the length of local storage (number of keys)
        /// </summary>
        /// <returns>The number length (Number of keys) of local storage</returns>
        Task<int> LengthAsync();

        /// <summary>
        /// Removes the item from local storage
        /// </summary>
        /// <param name="key">The unique key to use</param>
        /// <returns>The Async Task</returns>
        Task RemoveItemAsync(string key);

        /// <summary>
        /// Store the object in local storage using the key provided
        /// </summary>
        /// <param name="key">The unique key to use</param>
        /// <param name="data">The data to store</param>
        /// <param name="overwriteExisting">(Optional Defaults to true): If true any existing data for this key will be overwritten</param>
        /// <returns>True if successfully stored</returns>
        Task<bool> SetItemAsync(string key, object data, bool overwriteExisting = true);


    }
}
