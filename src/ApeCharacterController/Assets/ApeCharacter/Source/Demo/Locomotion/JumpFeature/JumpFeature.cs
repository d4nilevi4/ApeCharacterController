using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    public class JumpFeature : MonoFeature, IUpdatable
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private Rigidbody _rigidbody;
        
        private IApeCharacter _owner;

        public void OnUpdate(float dt)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpForce = 10f;
                var jumpVelocity = Vector3.up * _jumpForce;
                _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, jumpVelocity.y, _rigidbody.linearVelocity.z);
            }
        }
    }
}