using System;
using ApeCharacter.Injector;
using UnityEngine;

namespace ApeCharacter
{
    public class ApeSystemFactory : IApeSystemFactory
    {
        private readonly DiContainer _container;
        private readonly CharacterKernel _characterKernel;

        public ApeSystemFactory(
            DiContainer container,
            CharacterKernel characterKernel
        )
        {
            _container = container;
            _characterKernel = characterKernel;
        }

        public T CreateSystem<T>() where T : class, IApeSystem
        {
            return (T)CreateSystem(typeof(T));
        }

        public IApeSystem CreateSystem(Type systemType)
        {
            IApeSystem apeSystem;

            if (typeof(MonoBehaviour).IsAssignableFrom(systemType))
            {
                var component = _characterKernel.GetComponentInChildren(systemType, true);
            
                apeSystem = component as IApeSystem;
                if (apeSystem == null)
                    throw new System.Exception(
                        $"Компонент типа {systemType.Name} не найден среди детей CharacterKernel.");
            }
            else
            {
                apeSystem = (IApeSystem)_container.Instantiate(systemType);
            }

            if (apeSystem is IInitializable initializable)
                _characterKernel.AddInitializable(initializable);
            if (apeSystem is IUpdatable updatable)
                _characterKernel.AddUpdatable(updatable);
            if (apeSystem is IFixedUpdatable fixedUpdatable)
                _characterKernel.AddFixedUpdatable(fixedUpdatable);
            if (apeSystem is ILateUpdatable lateUpdatable)
                _characterKernel.AddLateUpdatable(lateUpdatable);

            return apeSystem;
        }
    }
}