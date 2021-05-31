using System;

namespace BLECoder.Blazor.LocalStorage
{
    /// <summary>
    /// The base local storage service
    /// </summary>
    public interface IBaseLocalStorageService
    {
        /// <summary>
        /// The event triggered when value in storage is changing.
        /// </summary>
        event EventHandler<ChangingEventArgs> Changing;

        /// <summary>
        /// Event to trigger when change is called.
        /// </summary>
        event EventHandler<ChangedEventArgs> Changed;
    }
}
