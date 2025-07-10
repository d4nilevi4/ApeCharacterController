using ApeCharacter.Demo.Locomotion;
using ApeCharacter.Injector;
using UnityEngine;

namespace ApeCharacter
{
    // TODO: Find more eligible place for registration and unregistration.
    [RequireComponent(typeof(CharacterKernel))]
    public abstract class ApeCharacterBase : MonoBehaviour, IApeCharacter
    {
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

            InjectMonoBehaviours();
        }

        private void InjectMonoBehaviours()
        {
            foreach (var component in GetComponentsInChildren<ComponentRegistrar>())
            {
                _container.InjectDependencies(component);
            }
        }

        protected abstract void InitializeSystems();

        protected void AddSystem<T>() where T : class, IApeSystem
        {
            var apeSystemFactory = _container.Resolve<IApeSystemFactory>();
            
            apeSystemFactory.CreateSystem<T>();
        }
        
        private void Start()
        {
            _container.InjectAll();
            
            foreach (IComponentRegistrar registrar in GetComponentsInChildren<IComponentRegistrar>())
                registrar.RegisterComponents();
            
            InitializeSystems();
        }

        private void OnDestroy()
        {
            foreach (IComponentRegistrar registrar in GetComponentsInChildren<IComponentRegistrar>())
                registrar.UnregisterComponents();
        }
    }
}