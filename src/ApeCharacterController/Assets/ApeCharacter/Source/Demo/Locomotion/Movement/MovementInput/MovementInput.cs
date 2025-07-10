using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class MovementInput : IMovementInput
    {
        public bool HasInput => InputAxis.sqrMagnitude > 0.01f;
        public Vector2 InputAxis => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}