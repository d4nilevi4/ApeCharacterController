using UnityEngine;

namespace ApeCharacter.Demo.Locomotion.StaticData
{
    [CreateAssetMenu( menuName = nameof(MovementConfig), fileName = nameof(MovementConfig))]
    public class MovementConfig : ScriptableObject, IApeCharacterConfig
    {
        public float Speed;
        public float Acceleration;
        public float Deceleration;
    }
}