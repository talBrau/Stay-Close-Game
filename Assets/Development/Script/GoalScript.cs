using System;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    #region Inspector

    [SerializeField] private SceneManager sceneManager;

    #endregion

    #region Fields

    private int _inGoalCounter;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter++;
        if (_inGoalCounter == 2)
            sceneManager.ChangeScene();
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter--;
    }

    #endregion
}
