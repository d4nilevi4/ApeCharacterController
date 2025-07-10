using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public interface IMovementInput
    {
        bool HasInput { get; }
        Vector2 InputAxis { get; }
    }
}