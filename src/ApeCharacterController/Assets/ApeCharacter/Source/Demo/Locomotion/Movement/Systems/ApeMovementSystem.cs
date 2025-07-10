using ApeCharacter.Demo.Physics;
using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class ApeMovementSystem : IApeSystem, IInitializable, IUpdatable
    {
        private readonly IApeCharacter _owner;
        
        private MovementTypeIdComponent _movementTypeId;
        private Rigidbody _rigidbody;
        private Speed _speed;
        private MovementInputAxis _inputAxis;
        private MovementAvailable _movementAvailable;

        public ApeMovementSystem(
            IApeCharacter owner
        )
        {
            _owner = owner;
        }
        
        public void Initialize()
        {
            _rigidbody = _owner.Components.GetComponent<RigidbodyComponent>().Value;
            _speed = _owner.Components.GetComponent<Speed>();
            _movementTypeId = _owner.Components.GetComponent<MovementTypeIdComponent>();
            _inputAxis = _owner.Components.GetComponent<MovementInputAxis>();
            _movementAvailable = _owner.Components.GetComponent<MovementAvailable>();
        }

        public void OnUpdate(float dt)
        {
            Vector3 velocity = new Vector3(_inputAxis.Value.x, 0, _inputAxis.Value.y);

            velocity = velocity * (_speed.Value);

            _rigidbody.linearVelocity = new Vector3(velocity.x, _rigidbody.linearVelocity.y, velocity.z);
        }
    }
}