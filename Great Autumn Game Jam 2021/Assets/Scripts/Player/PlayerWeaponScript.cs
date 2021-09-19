using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.UI.InGameOverlay;

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
                // Only shoot bullet if player has enough ammo.
                if(InGameViewModel.Corn > 0) {
                    GameObject newBullet = Instantiate(bullet, transform);
                    InGameViewModel.Corn--;
                }
            }
        }
    }
}