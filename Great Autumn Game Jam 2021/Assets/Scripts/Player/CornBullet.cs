using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts
{
    public class CornBullet : MonoBehaviour
    {
        private float bulletSpeed = 10.0f;

        /// <summary>
        /// Movement vector determining the bullet direction.
        /// </summary>
        private Vector2 _bulletMovement;

        void Start() {
            // move bullet in the direction that character is moving
            _bulletMovement.x = Input.GetAxisRaw("Horizontal");
            _bulletMovement.y = Input.GetAxisRaw("Vertical");

            // if character is not moving, shoot downward
            if(_bulletMovement.x == 0 && _bulletMovement.y == 0) {
                _bulletMovement.y = -1;
            }
        }

        /// <summary>
        /// Used for physics calculations
        /// </summary>
        private void FixedUpdate()
        {
            var cornRigidBody = GetComponent<Rigidbody2D>();
            cornRigidBody.MovePosition(cornRigidBody.position + _bulletMovement * bulletSpeed * Time.deltaTime);
        }
    }
}