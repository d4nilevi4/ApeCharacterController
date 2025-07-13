using System;
using System.Collections.Generic;

namespace ApeCharacter
{
    public interface IApeSystemsContainer
    {
        void AddSystem<T>(T system) where T : IApeSystem;
        void RemoveSystem<T>() where T : IApeSystem;
        bool TryGetSystem<T>(out T system) where T : IApeSystem;
        bool HasSystem<T>() where T : IApeSystem;
        T GetSystem<T>() where T : IApeSystem;
    }
}