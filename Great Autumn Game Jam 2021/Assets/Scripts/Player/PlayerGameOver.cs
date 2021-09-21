using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerGameOver : MonoBehaviour
    {
        public GameOverController gameOverController;

        private bool gameOver = false;

        private float delay = 2.0f;

        void Update()
        {
            if(gameOver) {
                delay -= Time.deltaTime;
                if(delay <= 0) {
                    gameOverController.Show();
                    // Reset [gameOver] value. The above should only
                    // be called once on a Game Over.
                    gameOver = false;
                }
            }
        }

        public void onGameOver() {
            gameOver = true;
        }
    }
}