using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    /// <summary>
    /// Next to load to.
    /// </summary>
    private string _nextSceneName;

    /// <summary>
    /// Set the scene that this portal switches to once activated.
    /// </summary>
    /// <param name="sceneName"></param> 
    public void SetNextScene(string sceneName) => _nextSceneName = sceneName;

    /// <summary>
    /// When an object collides with the portal
    /// </summary>
    /// <param name="collision"></param>
    [UsedImplicitly]
    private void OnTriggerEnter2D(Collider2D col)
    {
        var obj = col.gameObject;
        var playerMovementScript = obj.GetComponent<PlayerMovement>() ?? obj.GetComponentInChildren<PlayerMovement>();
        if (playerMovementScript == null) return;

        SceneManager.LoadScene(_nextSceneName);
    }

}
