using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region MonoBehaviour

    #endregion

    #region Methods

    public void ChangeLevel()
    {
        GetComponentInChildren<LevelChanger>().FadeOut();
    }

    #endregion
}
