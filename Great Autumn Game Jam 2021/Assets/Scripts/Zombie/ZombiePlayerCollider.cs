using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts
{
    public class ZombiePlayerCollider : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {   
            /// Triggered when the player enters the range of the zombie
            if (collision.gameObject.CompareTag("Player"))
            {
                // Kill player
                collision.gameObject.GetComponent<PlayerMovement>().die();
            }
        }
    }
}