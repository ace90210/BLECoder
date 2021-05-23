﻿using System;

namespace LocalStorage
{
    public interface IBaseLocalStorageService
    {
        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}