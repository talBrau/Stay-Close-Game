using UnityEngine;

public class GoalScript : MonoBehaviour
{
    #region Inspector
    
    #endregion

    #region Fields

    private int _inGoalCounter;


    #endregion

    #region MonoBehaviour

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter++;
        if (_inGoalCounter == 2)
        {
            GameManager.ChangeToNextLevelFlag = true;
            GameManager.InvokeFadeOut();
        }
    }*/

    public void ChangeLevel()
    {
        print("next level");
        GameManager.ChangeToNextLevelFlag = true;
        GameManager.InvokeFadeOut();
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("friend") && !col.gameObject.CompareTag("Player")) return;
        _inGoalCounter--;
    }

    #endregion#
}
