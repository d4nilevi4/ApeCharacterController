using System;
using System.Collections.Generic;

namespace ApeCharacter
{
    public sealed class ApeSystemsContainer : IApeSystemsContainer
    {
        private readonly Dictionary<Type, IApeSystem> _systems = new();
        
        public void AddSystem<T>(T system) where T : IApeSystem
        {
            if (system == null)
            {
                throw new ArgumentNullException(nameof(system), "System cannot be null");
            }

            Type systemType = typeof(T);

            if (!_systems.TryAdd(systemType, system))
            {
                throw new InvalidOperationException(
                    $"System of type {systemType.Name} already exists.");
            }
        }

        public void RemoveSystem<T>() where T : IApeSystem
        {
            Type systemType = typeof(T);

            if (!_systems.Remove(systemType))
            {
                throw new InvalidOperationException(
                    $"System of type {systemType.Name} does not exist.");
            }
        }

        public bool TryGetSystem<T>(out T system) where T : IApeSystem
        {
            Type systemType = typeof(T);

            if (_systems.TryGetValue(systemType, out IApeSystem foundSystem))
            {
                system = (T)foundSystem;
                return true;
            }

            system = default;
            return false;
        }

        public bool HasSystem<T>() where T : IApeSystem
        {
            return _systems.ContainsKey(typeof(T));
        }

        public T GetSystem<T>() where T : IApeSystem
        {
            Type systemType = typeof(T);

            if (_systems.TryGetValue(systemType, out IApeSystem foundSystem))
            {
                return (T)foundSystem;
            }

            throw new InvalidOperationException(
                $"System of type {systemType.Name} does not exist.");
        }
    }
}