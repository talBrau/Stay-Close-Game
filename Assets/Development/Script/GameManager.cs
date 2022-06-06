using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    #region Fields

    private int numberOfLevels;
    public static GameObject LastCheckPoint { get; set; }
    public static bool ChangeToNextLevelFlag { get; set; }
    public static int CurrentLevel { get; set; }

    public static bool FadingOut;
    
    
    #endregion

    #region Events

    public static event Action FadeOut;
    public static event Action CheckPointReset;
    public static event Action ChangeLevel;

    #endregion
    
    #region MonoBehaviour

    private void Start()
    {
        CurrentLevel = SceneManager.GetActiveScene().buildIndex;
        numberOfLevels = 4;
    }

    private void OnEnable()
    {
        ChangeLevel += ChangeToNextLevel;
    }

    private void OnDestroy()
    {
        ChangeLevel -= ChangeToNextLevel;
    }

    #endregion 

    #region Methods

    private void ChangeToNextLevel()
    {
        print(CurrentLevel);
        CurrentLevel++;
        if (CurrentLevel == numberOfLevels)
            CurrentLevel = 0;
        ChangeToNextLevelFlag = false;
        SceneManager.LoadScene(CurrentLevel);
    }
    public static void CheckPointInvoke()
    {
        CheckPointReset?.Invoke();
    }

    public static void InvokeFadeOut()
    {
        if (FadingOut) return;
        FadingOut = true;
        FadeOut?.Invoke();
    }

    public static void InvokeChangeLevel()
    {
        ChangeLevel?.Invoke();
    }

    public void ResetSceneButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeToNextLevelFlag = true;
            CurrentLevel = numberOfLevels - 1;
            InvokeFadeOut();
        }
    }
    public void ResetToCheckPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeToNextLevelFlag = false;
            InvokeFadeOut();
        }
    }
    
    
    #endregion
}
