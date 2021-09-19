using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Constants;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// Number of corn collected.
        /// </summary>
        public int CornCount { get; private set; }

        /// <summary>
        /// Rigid Body Reference
        /// </summary>
        private Rigidbody2D _playerRigidBody;

        /// <summary>
        /// Player Movement Speed
        /// </summary>
        [SerializeField]
        private readonly float movementSpeed = 4;

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
        /// UI In game view.
        /// </summary>
        private GameObject ui_inGameView;

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

        /// The player is killed. Darn.
        public void die() {
            // Lock player in place.
            _playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

            // Trigger death animation.
            _animator.SetTrigger("Death");

            // Show game over screen
            GetComponent<PlayerGameOver>().onGameOver();
        }

        /// <summary>
        /// On trigger enter
        /// </summary>
        /// <param name="trigger">Collider triggering collision</param>
        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.gameObject.name == ItemsConstants.CornItem) CollectCorn(trigger.gameObject);
        }

        /// <summary>
        /// Collect corn.
        /// Jimmy can crack corn, but I don't care.
        /// </summary>
        /// <param name="corn">Collected corn</param>
        private void CollectCorn(GameObject corn)
        {
            Destroy(corn);
            ui_inGameView ??= GameObject.Find(UiConstants.InGameViewName);
            if (ui_inGameView == null) return;

            var viewScript = ui_inGameView.GetComponent<InGameView>();
            viewScript.IncrementCorn();
        }
    }
}
