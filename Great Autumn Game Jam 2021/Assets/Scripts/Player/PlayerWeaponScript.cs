using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts
{
    public class PlayerWeaponScript : MonoBehaviour
    {
        public GameObject bullet;

        // Update is called once per frame
        void Update()
        {
            // If spacebar was pressed.
            if(Input.GetKeyDown("space")) {
                GameObject newBullet = Instantiate(bullet, transform);
            }
        }
    }
}