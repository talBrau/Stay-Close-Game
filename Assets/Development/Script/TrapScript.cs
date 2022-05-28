using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrapScript : MonoBehaviour
{
    public UnityEvent trapEvent;
    private bool activatedFlag;

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetTrap;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetTrap;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !activatedFlag)
        {
            trapEvent.Invoke();
            activatedFlag = true;
        }
    }

    private void ResetTrap()
    {
        activatedFlag = false;
    }
}
