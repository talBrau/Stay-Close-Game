using UnityEngine;

public class GoalScript : MonoBehaviour
{
    #region Inspector
    
    #endregion

    #region Fields

    private int _inGoalCounter;
    private SceneManager _sceneManager;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter++;
        if (_inGoalCounter == 2)
            _sceneManager.ChangeLevel();
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter--;
    }

    #endregion
}
