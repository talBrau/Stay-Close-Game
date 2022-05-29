using System;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
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

    private void Start()
    {
        FadeIn();
    }

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
        if (GameManager.ChangeToNextLevelFlag)
        {
            GameManager.InvokeChangeLevel();
        }
        else
            GameManager.CheckPointInvoke();
    }

}
