using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class ReturnFriend : MonoBehaviour
{
    [SerializeField] private GameObject friend;
    private bool _done = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(returnFriend), 1.5f);
        }
    }

    private void returnFriend()
    {
        if (_done) return;

        friend.GetComponent<FriendController>().friendState = FriendController.FriendState.Idle;
        friend.transform.position = transform.position + Vector3.up;
        _done = true;
    }
}