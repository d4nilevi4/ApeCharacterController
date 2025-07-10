using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class ApeMovementInitializeSystem : IApeSystem, IInitializable
    {
        private readonly IApeCharacter _owner;

        public ApeMovementInitializeSystem(
            IApeCharacter owner
        )
        {
            _owner = owner;
        }
        
        public void Initialize()
        {
            _owner.Components.AddComponent(new Speed(){Value = 4.0f});
            _owner.Components.AddComponent(new MovementTypeIdComponent(){Value = MovementTypeId.Walk});
            _owner.Components.AddComponent(new MovementInputAxis());
            _owner.Components.AddComponent(new MovementAvailable());
        }
    }
}