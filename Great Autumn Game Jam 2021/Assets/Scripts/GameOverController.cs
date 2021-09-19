using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        public void Show() {
            gameObject.SetActive(true);
        }

        public void TryAgainButton() {
            SceneManager.LoadScene("Maze_Easy_1");
        }
    }
}
