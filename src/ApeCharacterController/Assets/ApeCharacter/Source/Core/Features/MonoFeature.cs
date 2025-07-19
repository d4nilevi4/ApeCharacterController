using System;
using ApeCharacter.Injector;
using UnityEngine;

namespace ApeCharacter
{
    [Serializable]
    public abstract class MonoFeature : MonoBehaviour, IApeFeature
    {
        private IApeSystemFactory _systemFactory;
        private IApeCharacter _owner;

        [Inject]
        private void Construct(
            IApeSystemFactory systemFactory,
            IApeCharacter owner
        )
        {
            Debug.Log("Construct");
            
            _systemFactory = systemFactory;
            _owner = owner;
        }
        
        public void Add<T>() where T : class, IApeSystem
        {
            _owner.Systems.AddSystem(_systemFactory.CreateSystem<T>());
        }
    }
}