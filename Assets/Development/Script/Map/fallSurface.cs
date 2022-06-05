using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallSurface : MonoBehaviour
{
    private bool _entered = false;

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetSurface;
    }

    private void OnDisable()
    {
        GameManager.CheckPointReset -= ResetSurface;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !_entered)
        {
            _entered = true;
            GameManager.ChangeToNextLevelFlag = false;
            GameManager.InvokeFadeOut();
        }
    }

    private void ResetSurface()
    {
        _entered = false;
    }
}