using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class EmitMovementInputSystem : IApeSystem, IInitializable, IUpdatable
    {
        private readonly IApeCharacter _owner;
        private readonly IMovementInput _movementInput;
        private MovementInputAxis _inputAxis;
        private Camera _camera;

        public EmitMovementInputSystem(
            IApeCharacter owner,
            IMovementInput movementInput
        )
        {
            _owner = owner;
            _movementInput = movementInput;
        }

        public void Initialize()
        {
            _camera = Camera.main;
            _inputAxis = _owner.Components.GetComponent<MovementInputAxis>();
        }

        public void OnUpdate(float dt)
        {
            if (_movementInput.HasInput)
            {
                Vector2 worldInput = _movementInput.InputAxis;
                Vector3 forward = _camera.transform.forward;
                Vector3 right = _camera.transform.right;

                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();

                Vector3 relativeMovement = (forward * worldInput.y + right * worldInput.x);
                _inputAxis.Value = new Vector2(relativeMovement.x, relativeMovement.z);
            }
            else
                _inputAxis.Value = Vector2.zero;
        }
    }
}