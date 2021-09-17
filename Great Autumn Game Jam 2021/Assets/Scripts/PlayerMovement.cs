using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// Rigid Body Reference
        /// </summary>
        private Rigidbody2D _playerRigidBody;

        /// <summary>
        /// Player Movement Speed
        /// </summary>
        [SerializeField]
        private readonly float movementSpeed = 5;

        /// <summary>
        /// Animator used to animate character.
        /// </summary>
        [SerializeField]
        private Animator _animator;

        private Vector2 _movement;

        void Start()
        {
            // Get the rididbody component that is attached to our player
            _playerRigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetFloat("Vertical", _movement.y);
            _animator.SetFloat("Speed", _movement.sqrMagnitude);
        }

        /// <summary>
        /// Used for physics calculations
        /// </summary>
        [UsedImplicitly]
        private void FixedUpdate()
        {
            _playerRigidBody.MovePosition(_playerRigidBody.position + _movement * movementSpeed * Time.deltaTime);
        }
    }
}
