using ApeCharacter.Demo.Locomotion.StaticData;

namespace ApeCharacter.Demo.Locomotion
{
    public class ApeMovementInitializeSystem : IApeSystem, IInitializable
    {
        private readonly IApeCharacter _owner;
        private readonly ICharacterStaticDataProvider _characterStaticDataProvider;

        public ApeMovementInitializeSystem(
            IApeCharacter owner,
            ICharacterStaticDataProvider characterStaticDataProvider
        )
        {
            _owner = owner;
            _characterStaticDataProvider = characterStaticDataProvider;
        }
        
        public void Initialize()
        {
            MovementConfig movementConfig = _characterStaticDataProvider.GetConfig<MovementConfig>();

            _owner.Components.AddComponent(new Speed(){Value = movementConfig.Speed});
            _owner.Components.AddComponent(new MovementTypeIdComponent(){Value = MovementTypeId.Walk});
            _owner.Components.AddComponent(new MovementInputAxis());
            _owner.Components.AddComponent(new MovementAvailable());
        }
    }
}