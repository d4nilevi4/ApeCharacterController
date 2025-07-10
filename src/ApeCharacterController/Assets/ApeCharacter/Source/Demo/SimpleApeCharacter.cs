namespace ApeCharacter.Demo.Locomotion
{
    public class SimpleApeCharacter : ApeCharacterBase
    {
        protected override void InitializeSystems()
        {
            AddSystem<MovementFeature>();
        }
    }
}