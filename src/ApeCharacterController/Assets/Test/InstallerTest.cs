using ApeCharacter;
using ApeCharacter.Injector;

public class InstallerTest : MonoInstaller
{
    public ApeCharacterSOProvider Provider;
    
    public override void InstallBindings()
    {
        Container.BindFromInstance<ICharacterStaticDataProvider>(Provider);
    }
}