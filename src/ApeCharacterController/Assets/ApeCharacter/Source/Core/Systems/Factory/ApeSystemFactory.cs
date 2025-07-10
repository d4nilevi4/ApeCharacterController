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
            T apeSystem;

            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                var component = _characterKernel.GetComponentInChildren(typeof(T), true);

                apeSystem = component as T;
                if (apeSystem == null)
                    throw new System.Exception(
                        $"Компонент типа {typeof(T).Name} не найден среди детей CharacterKernel.");
            }
            else
            {
                apeSystem = _container.Instantiate<T>();
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