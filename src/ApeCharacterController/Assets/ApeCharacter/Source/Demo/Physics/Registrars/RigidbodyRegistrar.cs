using UnityEngine;

namespace ApeCharacter.Demo.Physics
{
    public class RigidbodyRegistrar : ComponentRegistrar
    {
        public Rigidbody Rigidbody;

        public override void RegisterComponents()
        {
            Components.AddComponent(new RigidbodyComponent(){Value = Rigidbody});
        }

        public override void UnregisterComponents()
        {
            if (Components.HasComponent<RigidbodyComponent>())
                Components.RemoveComponent<RigidbodyComponent>();
        }
    }
}