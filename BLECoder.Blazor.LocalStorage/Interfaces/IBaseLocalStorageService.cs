using System;

namespace BLECoder.Blazor.LocalStorage
{
    public interface IBaseLocalStorageService
    {
        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
