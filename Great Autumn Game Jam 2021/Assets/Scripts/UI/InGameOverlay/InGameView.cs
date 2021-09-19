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

    // Update is called once per frame
    void Update()
    {
        var label = _cornLabel.GetComponent<Text>() ?? _cornLabel.GetComponentInChildren<Text>();
        if (label == null) return;

        label.text = $"Corn: {InGameViewModel.Corn}";
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
    }
}
