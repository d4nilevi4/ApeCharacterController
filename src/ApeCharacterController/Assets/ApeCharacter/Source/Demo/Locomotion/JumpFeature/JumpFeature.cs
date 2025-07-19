using System;
using UnityEngine;

namespace ApeCharacter.Demo.Locomotion
{
    [Serializable]
    public class JumpFeature : MonoFeature
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private Rigidbody _rigidbody;
        
        public void Update()
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