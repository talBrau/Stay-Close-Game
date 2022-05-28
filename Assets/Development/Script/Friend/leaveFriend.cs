using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaveFriend : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerManager>().SetFreezeDistance(Mathf.Infinity);
    }
}