using System;

namespace ApeCharacter
{
    public interface IApeBusyTracker
    {
        event Action<Type> BusyChanged;
        Type Busy { get; }
        void SetBusy<T>() where T : class, IApeSystem;
        void SetForceBusy<T>() where T : class, IApeSystem;
        void SetFree();
    }
}