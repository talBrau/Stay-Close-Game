using System.Collections.Generic;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneManager : MonoBehaviour
{
    #region MonoBehaviour
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Fields

    private int _sceneCounter;

    #endregion
    
    #region Methods

    public void ChangeScene()
    {
        _sceneCounter++;
        if (_sceneCounter == UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            print("Good Job");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneCounter);
    }

    #endregion
}
