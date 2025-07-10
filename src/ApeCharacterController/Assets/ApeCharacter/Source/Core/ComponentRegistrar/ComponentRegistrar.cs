using ApeCharacter.Injector;
using UnityEngine;

namespace ApeCharacter
{
    public abstract class ComponentRegistrar : MonoBehaviour, IComponentRegistrar
    {
        private IApeCharacter _owner;
        
        protected IApeComponentsContainer Components => _owner.Components;

        [Inject]
        public void Construct(IApeCharacter owner)
        {
            _owner = owner;
        }

        public abstract void RegisterComponents();
        public abstract void UnregisterComponents();
    }
}