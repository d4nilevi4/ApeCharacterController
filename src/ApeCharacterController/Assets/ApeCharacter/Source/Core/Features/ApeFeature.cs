using System;
using UnityEngine;

namespace ApeCharacter
{
    [Serializable]
    public abstract class ApeFeature : IApeFeature
    {
        private readonly IApeSystemFactory _systemFactory;
        private readonly IApeCharacter _owner;

        [SerializeField] protected bool Feature;
        
        protected ApeFeature(
            IApeSystemFactory systemFactory,
            IApeCharacter owner
        )
        {
            _systemFactory = systemFactory;
            _owner = owner;
        }
        
        public void Add<T>() where T : class, IApeSystem
        {
            _owner.Systems.AddSystem(_systemFactory.CreateSystem<T>());
        }
    }
}