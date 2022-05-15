using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    
    #region Fields

    public static GameObject LastCheckPoint { get; set; }

    #endregion

    #region Events

    public static event Action FadeOut;
    public static event Action CheckPointReset;

    #endregion
    
    #region MonoBehaviour

    #endregion

    #region Methods

    public static void CheckPointInvoke()
    {
        CheckPointReset?.Invoke();
    }

    public static void InvokeFadeOut()
    {
        FadeOut?.Invoke();
    }
    
    

    public void ResetToCheckPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
            InvokeFadeOut();
    }
    
    #endregion
}
