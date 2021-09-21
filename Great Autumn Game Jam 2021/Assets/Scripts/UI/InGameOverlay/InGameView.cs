using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.InGameOverlay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InGameView : MonoBehaviour
{
    /// <summary>
    /// Corn Label
    /// </summary>
    [SerializeField] private GameObject _cornLabel;

    /// <summary>
    /// Level Label
    /// </summary>
    [SerializeField] private GameObject _levelLabel;

    // Update is called once per frame
    void Update()
    {
        var cornText = _cornLabel.GetComponent<Text>() ?? _cornLabel.GetComponentInChildren<Text>();
        var levelText = _levelLabel.GetComponent<Text>() ?? _levelLabel.GetComponentInChildren<Text>();
        if (cornText == null || levelText == null) return;

        cornText.text = $"Corn: {InGameViewModel.Corn}";
        levelText.text = $"Level {InGameViewModel.Level}";
    }

    /// <summary>
    /// Increase the corn count by 1
    /// </summary>
    public void IncrementCorn()
    {
        InGameViewModel.Corn++;
    }

    /// <summary>
    /// Decrease the corn count by 1
    /// </summary>
    public void DecrementCorn()
    {
        InGameViewModel.Corn--;
    }

    /// <summary>
    /// Reset View Data
    /// </summary>
    public void ResetView()
    {
        InGameViewModel.Corn = 0;
        InGameViewModel.Level = 1;
    }
}
