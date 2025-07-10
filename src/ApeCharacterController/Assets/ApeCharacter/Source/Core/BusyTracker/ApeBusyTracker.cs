using System;
using UnityEngine;

namespace ApeCharacter
{
    public class ApeBusyTracker : IApeBusyTracker
    {
        private readonly ILogger _logger;
        private Type _busy;
        
        public event Action<Type> BusyChanged;

        public ApeBusyTracker(
            ILogger logger
        )
        {
            _logger = logger;
        }

        public Type Busy
        {
            get => _busy;
            private set
            {
                if (_busy == value) 
                    return;
                
                _busy = value;
                BusyChanged?.Invoke(value);
            }
        }

        public void SetBusy<T>() where T : class, IApeSystem
        {
            if (Busy != null)
            {
                _logger.LogWarning(
                    tag: "(0_0)",
                    message: $"Character is already busy: {Busy.Name}");
                return;
            }

            SetForceBusy<T>();
        }

        public void SetForceBusy<T>() where T : class, IApeSystem => 
            Busy = typeof(T);

        public void SetFree() =>
            Busy = null;
    }
}