using System;

namespace ApeCharacter
{
    public interface IApeSystemFactory
    {
        T CreateSystem<T>() where T : class, IApeSystem;
        IApeSystem CreateSystem(Type systemType);
    }
}