using ApeCharacter.Injector;

namespace ApeCharacter
{
    public class StaticDataInstaller : MonoInstaller
    {
        public ApeCharacterSOProvider Provider;
    
        public override void InstallBindings()
        {
            Container.BindFromInstance<ICharacterStaticDataProvider>(Provider);
        }
    }
}