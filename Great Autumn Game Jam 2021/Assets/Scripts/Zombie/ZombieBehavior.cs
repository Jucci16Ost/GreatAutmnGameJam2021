using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ZombieBehavior : MonoBehaviour
    {
    	/// <summary>
        /// Rigid Body Reference
        /// </summary>
        private Rigidbody2D _zombieRigidBody;

    	/// <summary>
    	/// Player Movement Speed
    	/// </summary>
    	[SerializeField]
    	private readonly float movementSpeed = 1f;

        /// <summary>
        /// Animator used to animate character.
        /// </summary>
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// Movement vector
        /// </summary>
        private Vector2 _movement;

        /// <summary>
        /// True if the zombie is within proximity of the player, and should be 
        /// chasing them.
        /// </summary>
        private bool _inProximityToPlayer = false;
        
        /// <summary>
        /// Timer that determines when the next action will occur for an idle zombie.
        /// </summary>
        private float _randomMovementTimer;

        /// <summary>
        /// Determines if the zombie is defeated.
        /// </summary>
        private bool isDefeated = false;

        /// <summary>
        /// Countdown Timer to remove this GameObject after the zombies defeat.
        /// </summary>
        private float _removeAfterDefeatTimer = 2.0f;


        // Start is called before the first frame update
        void Start()
        {
            _randomMovementTimer = 3.0f;

            // Get the rididbody component that is attached to the zombie
            _zombieRigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // Check if zombie is defeated
            checkIfDefeated();

            // If zombie is idle.
            if(!_inProximityToPlayer) {
                _idleMovement();
            }

            /// Animate zombie.
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
            _zombieRigidBody.MovePosition(_zombieRigidBody.position + _movement * movementSpeed * Time.deltaTime);
        }

        void OnTriggerStay2D(Collider2D collision)
        {   
            /// Triggered when the player enters the range of the zombie
            if (collision.gameObject.CompareTag("Player"))
            {
                _inProximityToPlayer = true;
                _huntDownPlayer(collision.gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            /// Triggered when the player leaves the range of the zombie
            if (collision.gameObject.CompareTag("Player"))
            {
                _inProximityToPlayer = false;
                _movement.x = 0;
                _movement.y = 0;
            }
        }

        private void _huntDownPlayer(GameObject player) {
            float xDiff = player.GetComponent<Rigidbody2D>().position.x - _zombieRigidBody.position.x;
            float yDiff = player.GetComponent<Rigidbody2D>().position.y - _zombieRigidBody.position.y;

            // If X difference is too small, don't move in the horiztonal direction
            if (xDiff <= 0.1 && xDiff >= -0.1) {
                _movement.x = 0;
            } else {
                _movement.x = (xDiff > 0) ? 1 : -1;
            }

            // If Y difference is too small, don't move in the vertical direction
            if (yDiff <= 0.1 && yDiff >= -0.1) {
                _movement.y = 0;
            } else {
                _movement.y = (yDiff > 0) ? 1 : -1;
            }
        }

        /// Controls random movements while the zombie is idle.
        private void _idleMovement() {
            _randomMovementTimer -= Time.deltaTime;
            // If timer is up, switch idle movement type.
            if(_randomMovementTimer <= 0) {
                // If the player was not moving, start walking in a random direction.
                if(_movement.x == 0 && _movement.y == 0) {
                    _randomMovementTimer = 0.5f;
                    if(_randomBool()) {
                        _movement.x = Random.Range(-1, 2); // between -1 and 1 inclusive
                    } else {
                        _movement.y = Random.Range(-1, 2); // between -1 and 1 inclusive
                    }
                }
                // // If the player was just moving, stop and switch to idle stance.
                else {
                    _randomMovementTimer = 2.5f;
                    _movement.x = 0;
                    _movement.y = 0;
                }
            }
        }

        private bool _randomBool() {
            return (Random.value > 0.5f);
        }

        private void checkIfDefeated() {
            if(isDefeated) {
                _removeAfterDefeatTimer -= Time.deltaTime;
                if(_removeAfterDefeatTimer <= 0) {
                    Destroy(this.gameObject);
                }
            }
        }

        /// The zombie is defeated. Woohoo!
        public void defeat() {
            // Lock zombie in place.
            _zombieRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

            // Disable PlayerCollider so that the player no longer dies on impact
            transform.Find("PlayerCollider").gameObject.SetActive(false);
            var component = GetComponent<Collider2D>();
            if (component != null) component.enabled = false;

            // Trigger death animation.
            _animator.SetTrigger("Death");

            // Set delay for dead zombie removal
            isDefeated = true;
        }
    }
}