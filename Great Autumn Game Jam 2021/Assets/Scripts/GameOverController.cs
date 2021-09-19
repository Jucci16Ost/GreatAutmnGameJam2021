using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        public void Show() {
            gameObject.SetActive(true);
            var inGameView = GameObject.Find(UiConstants.InGameViewName);
            inGameView?.SetActive(false);
        }

        public void TryAgainButton() {
            SceneManager.LoadScene("Maze_Easy_1");
        }
    }
}
