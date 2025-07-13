using System;
using System.Collections.Generic;
using ApeCharacter.Demo.Locomotion;
using ApeCharacter.Injector;
using UnityEngine;

namespace ApeCharacter
{
    [System.Serializable]
    public class FeatureTypeReference
    {
        public string TypeName;

        public FeatureTypeReference(Type typeName)
        {
            TypeName = typeName.FullName;
        }
    }
    
    // TODO: Find more eligible place for registration and unregistration.
    [RequireComponent(typeof(CharacterKernel))]
    public abstract class ApeCharacterBase : MonoBehaviour, IApeCharacter
    {
        public List<FeatureTypeReference> FeatureTypes = new();
        
        private DiContainer _container;
        
        public IApeSystemsContainer Systems { get; private set; } = 
            new ApeSystemsContainer();

        public IApeComponentsContainer Components { get; private set; } =
            new ApeComponentsContainer();

        private void Awake()
        {
            _container = new DiContainer();
            
            _container.BindFromInstance<IApeCharacter>(this);
            _container.BindFromInstance<IApeSystemsContainer>(Systems);
            _container.BindFromInstance<IApeComponentsContainer>(Components);
            _container.BindFromInstance(GetComponent<CharacterKernel>());
            
            _container.Bind<IApeSystemFactory, ApeSystemFactory>();
            _container.Bind<IMovementInput, MovementInput>();
            
            _container.InjectAll();
            InjectMonoBehaviours();
            
            foreach (IComponentRegistrar registrar in GetComponentsInChildren<IComponentRegistrar>())
                registrar.RegisterComponents();
            
            InitializeSystems();

            new SystemsValidator().ValidateSystems(this, transform);
            
        }

        private void InjectMonoBehaviours()
        {
            foreach (var component in GetComponentsInChildren<MonoBehaviour>())
            {
                _container.InjectDependencies(component);
            }
        }

        private void InitializeSystems()
        {
            foreach (var systemTypeRef in FeatureTypes)
            {
                Type systemType = GetTypeByName(systemTypeRef.TypeName);
                if (systemType == null)
                {
                    Debug.LogWarning($"Не удалось найти тип {systemTypeRef.TypeName}");
                    continue;
                }
        
                if (!typeof(IApeSystem).IsAssignableFrom(systemType))
                {
                    Debug.LogWarning($"Тип {systemTypeRef.TypeName} не реализует интерфейс IApeSystem");
                    continue;
                }

                AddSystem(systemType);
            }
        }

        private Type GetTypeByName(string typeName)
        {
            // Сначала пробуем стандартный метод
            Type type = Type.GetType(typeName);
            if (type != null)
                return type;
    
            // Если не получилось, ищем в загруженных сборках
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);
                if (type != null)
                    return type;
            }
    
            return null;
        }

        protected void AddSystem<T>() where T : class, IApeSystem
        {
            var apeSystemFactory = _container.Resolve<IApeSystemFactory>();
            
            apeSystemFactory.CreateSystem<T>();
        }

        protected void AddSystem(Type systemType)
        {
            if (!typeof(IApeSystem).IsAssignableFrom(systemType))
            {
                throw new Exception();
            }
            
            var apeSystemFactory = _container.Resolve<IApeSystemFactory>();
            
            apeSystemFactory.CreateSystem(systemType);
        }

        private void OnDestroy()
        {
            foreach (IComponentRegistrar registrar in GetComponentsInChildren<IComponentRegistrar>())
                registrar.UnregisterComponents();
        }
    }
}