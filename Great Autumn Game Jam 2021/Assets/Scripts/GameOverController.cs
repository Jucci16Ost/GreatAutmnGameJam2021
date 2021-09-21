using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.DayNightCycle;
using Assets.Scripts.UI.InGameOverlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        private GameObject _inGameView;

        public void Show() {
            // show screen
            gameObject.SetActive(true);

            // update level reached text
            var levelReachedView = GameObject.Find(UiConstants.GameOverLevelReachedName);
            levelReachedView.GetComponent<Text>().text = $"You survived until Level {InGameViewModel.Level}";
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
