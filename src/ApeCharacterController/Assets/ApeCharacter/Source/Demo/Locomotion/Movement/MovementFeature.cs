namespace ApeCharacter.Demo.Locomotion
{
    public class MovementFeature : ApeFeature
    {
        public MovementFeature(IApeSystemFactory systemFactory, IApeCharacter owner)
            : base(systemFactory, owner)
        {
            Add<ApeMovementInitializeSystem>();
            
            Add<EmitMovementInputSystem>();

            Add<RotationSystem>();
            Add<ApeMovementSystem>();
        }
    }
}