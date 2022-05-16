using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    #region Fields

    public static GameObject LastCheckPoint { get; set; }
    public static bool ResetSceneFlag { get; set; }

    #endregion

    #region Events

    public static event Action FadeOut;
    public static event Action CheckPointReset;
    public static event Action ResetScene;

    #endregion
    
    #region MonoBehaviour

    private void OnEnable()
    {
        ResetScene += LoadScene;
    }

    private void OnDestroy()
    {
        ResetScene -= LoadScene;
    }

    #endregion

    #region Methods

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void CheckPointInvoke()
    {
        CheckPointReset?.Invoke();
    }

    public static void InvokeFadeOut()
    {
        FadeOut?.Invoke();
    }

    public static void InvokeResetScene()
    {
        ResetScene?.Invoke();
    }

    public void ResetSceneButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetSceneFlag = true;
            InvokeFadeOut();
        }
    }
    public void ResetToCheckPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetSceneFlag = false;
            InvokeFadeOut();
        }
    }
    
    #endregion
}
