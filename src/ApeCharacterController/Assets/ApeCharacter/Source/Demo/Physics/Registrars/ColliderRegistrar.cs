using UnityEngine;

namespace ApeCharacter.Demo.Physics
{
    public class ColliderRegistrar : ComponentRegistrar
    {
        public Collider Collider;

        public override void RegisterComponents()
        {
            Components.AddComponent(new ColliderComponent(){Value = Collider});
        }

        public override void UnregisterComponents()
        {
            if (Components.HasComponent<ColliderComponent>())
                Components.RemoveComponent<ColliderComponent>();
        }
    }
}