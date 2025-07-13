using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApeCharacter.Injector
{
    public class DiContainer
    {
        private readonly Dictionary<Type, Type> _bindings = new();
        private readonly Dictionary<Type, object> _singletons = new();

        public DiContainer()
        {
            _singletons[typeof(DiContainer)] = this;
        }
        
        public void Bind<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _bindings[typeof(TInterface)] = typeof(TImplementation);
        }

        public void BindSelf<T>() => _bindings[typeof(T)] = typeof(T);

        public void BindFromInstance<T>(T instance)
        {
            _singletons[typeof(T)] = instance;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T));

        public void InjectDependencies(object instance)
        {
            InjectDependencies(instance, instance.GetType());
        }
        
        public void InjectAll()
        {
            foreach (var pair in _singletons)
            {
                InjectDependencies(pair.Value, pair.Value.GetType());
            }
        }

        public object Instantiate(Type type)
        {
            var constructors = type.GetConstructors();
            var injectCtor = constructors.FirstOrDefault(c =>
                                 c.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                             ?? constructors.OrderByDescending(c => c.GetParameters().Length)
                                 .First();

            var parameters = injectCtor.GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            var instance = injectCtor.Invoke(parameters);

            _singletons[type] = instance;

            InjectDependencies(instance, type);

            return instance;
        }
        
        public T Instantiate<T>()
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();
            var injectCtor = constructors.FirstOrDefault(c =>
                                 c.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                             ?? constructors.OrderByDescending(c => c.GetParameters().Length)
                                 .First();

            var parameters = injectCtor.GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            var instance = injectCtor.Invoke(parameters);

            _singletons[type] = instance;

            InjectDependencies(instance, type);

            return (T)instance;
        }

        private object Resolve(Type type)
        {
            if (_singletons.TryGetValue(type, out var existingInstance))
                return existingInstance;

            if (_bindings.TryGetValue(type, out var implementationType))
                type = implementationType;

            var constructors = type.GetConstructors();
            var injectCtor = constructors.FirstOrDefault(c =>
                                 c.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                             ?? constructors.OrderByDescending(c => c.GetParameters().Length)
                                 .First();

            var parameters = injectCtor.GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            var instance = injectCtor.Invoke(parameters);

            _singletons[type] = instance;

            InjectDependencies(instance, type);

            return instance;
        }

        private void InjectDependencies(object instance, Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                   BindingFlags.NonPublic))
            {
                if (method.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                {
                    var methodParams = method.GetParameters()
                        .Select(p => Resolve(p.ParameterType))
                        .ToArray();
                    method.Invoke(instance, methodParams);
                }
            }
        }
    }
}