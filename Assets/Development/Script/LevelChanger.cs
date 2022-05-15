using System;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    /*private int _sceneCounter;*/
    /*private bool _changeLevelFlag;*/

    [SerializeField] private GameObject vCamera; 
    private void OnEnable()
    {
        GameManager.FadeOut += DisableCamera;
        GameManager.FadeOut += FadeOut;
        GameManager.CheckPointReset += ActiveCamera;
        GameManager.CheckPointReset += FadeIn;
    }

    private void OnDestroy()
    {
        GameManager.FadeOut -= DisableCamera;
        GameManager.FadeOut -= FadeOut;
        GameManager.CheckPointReset -= ActiveCamera;
        GameManager.CheckPointReset -= FadeIn;
    }

    /*private void Start()
    {
        _sceneCounter = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }*/

    private void DisableCamera()
    {
        vCamera.SetActive(false);
    }

    private void ActiveCamera()
    {
        vCamera.SetActive(true);
    }
    private void FadeOut()
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

    private void FadeIn()
    {
        GetComponent<Animator>().SetTrigger("FadeIn");
    }
    
    public void OnFadeOutComplete()
    {
        GameManager.CheckPointInvoke();
    }
    
    /*private void ChangeScene()
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
    }*/

}
