using System;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    private int _sceneCounter;
    private bool _changeLevelFlag;

    private void Start()
    {
        _sceneCounter = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    public void FadeOut(bool resetLevelFlag)
    {
        _changeLevelFlag = resetLevelFlag;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }
    
    public void OnFadeOutComplete()
    {
        ChangeScene();
    }
    private void ChangeScene()
    {
        if (_changeLevelFlag)
        {
            _sceneCounter++;
            if (_sceneCounter == UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                _sceneCounter = 0;
            }
            _changeLevelFlag = false;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneCounter);
    }

}
