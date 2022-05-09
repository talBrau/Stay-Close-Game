using System;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    private int _sceneCounter;

    private void Start()
    {
        _sceneCounter = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    public void FadeOut()
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
    }
    
    public void OnFadeOutComplete()
    {
        ChangeScene();
    }
    private void ChangeScene()
    {
        _sceneCounter++;
        if (_sceneCounter == UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            _sceneCounter = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneCounter);
    }

}
