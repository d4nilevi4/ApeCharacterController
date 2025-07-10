using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{ 
    public class Speed : IApeComponent { public float Value; }
    public class MovementTypeIdComponent : IApeComponent { public MovementTypeId Value; }
    public class MovementInputAxis : IApeComponent { public Vector2 Value; }
    public class MovementAvailable : IApeComponent {}
}