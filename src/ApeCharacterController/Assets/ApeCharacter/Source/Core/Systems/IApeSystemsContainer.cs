using System;
using System.Collections.Generic;

namespace ApeCharacter
{
    // TODO: Add all non generic methods
    public interface IApeSystemsContainer
    {
        void AddSystem<T>(T system) where T : IApeSystem;
        void RemoveSystem<T>() where T : IApeSystem;
        
        bool TryGetSystem<T>(out T system) where T : IApeSystem;
        bool TryGetSystem(Type systemType, out IApeSystem system);
        
        bool HasSystem<T>() where T : IApeSystem;
        bool HasSystem(Type systemType);
        
        T GetSystem<T>() where T : IApeSystem;
    }
}