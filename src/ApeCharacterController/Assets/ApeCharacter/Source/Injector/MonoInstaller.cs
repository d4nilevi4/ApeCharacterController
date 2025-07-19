using UnityEngine;

namespace ApeCharacter.Injector
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public DiContainer Container { get; set; }
        public abstract void InstallBindings();
    }
}