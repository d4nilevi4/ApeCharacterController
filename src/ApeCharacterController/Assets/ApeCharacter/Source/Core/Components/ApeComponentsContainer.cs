using System;
using System.Collections.Generic;

namespace ApeCharacter
{
    public sealed class ApeComponentsContainer : IApeComponentsContainer
    {
        private readonly Dictionary<Type, IApeComponent> _components = new();
        
        public void AddComponent<T>(T component) where T : IApeComponent
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component), "Component cannot be null");
            }

            Type componentType = typeof(T);

            if (!_components.TryAdd(componentType, component))
            {
                throw new InvalidOperationException(
                    $"Component of type {componentType.Name} already exists.");
            }
        }

        public void RemoveComponent<T>() where T : IApeComponent
        {
            Type componentType = typeof(T);

            if (!_components.Remove(componentType))
            {
                throw new InvalidOperationException(
                    $"Component of type {componentType.Name} does not exist.");
            }
        }

        public bool TryGetComponent<T>(out T component) where T : IApeComponent
        {
            Type componentType = typeof(T);

            if (_components.TryGetValue(componentType, out IApeComponent foundComponent))
            {
                component = (T)foundComponent;
                return true;
            }

            component = default;
            return false;
        }

        public bool HasComponent<T>() where T : IApeComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public T GetComponent<T>() where T : IApeComponent
        {
            Type componentType = typeof(T);

            if (_components.TryGetValue(componentType, out IApeComponent foundComponent))
            {
                return (T)foundComponent;
            }

            throw new InvalidOperationException(
                $"Component of type {componentType.Name} does not exist.");
        }
    }
}