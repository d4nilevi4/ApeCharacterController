using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class RotationSystem : MonoBehaviour, IApeSystem, IUpdatable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _rotationSpeed = 10f;
        
        public void OnUpdate(float dt)
        {
            float mouseX = Input.GetAxis("Mouse X");

            if (Mathf.Abs(mouseX) < 0.01f)
                return;

            float rotationAmount = mouseX * _rotationSpeed * dt;

            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);

            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }
}