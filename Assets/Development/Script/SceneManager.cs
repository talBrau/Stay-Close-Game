using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region MonoBehaviour

    #endregion

    #region Methods

    public void ChangeLevel(bool resetLevelFlag)
    {
        GetComponentInChildren<LevelChanger>().FadeOut(resetLevelFlag);
    }

    #endregion
}
