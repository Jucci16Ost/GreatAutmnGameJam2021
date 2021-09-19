using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.DayNightCycle;
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

            ResetView();
            ResetTime();
        }

        /// <summary>
        /// Reset View Data.
        /// </summary>
        private void ResetView()
        {
            var inGameView = GameObject.Find(UiConstants.InGameViewName);
            var viewScript = inGameView.GetComponent<InGameView>();
            viewScript.ResetView();
        }

        /// <summary>
        /// Reset the time of day
        /// </summary>
        private void ResetTime()
        {
            DayNightCycleContext.TimeOfDay = 11;
        }
    }
}
