using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class ReturnFriend : MonoBehaviour
{
    [SerializeField] private GameObject friend;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            friend.GetComponent<FriendController>().friendState = FriendController.FriendState.Idle;
            friend.transform.position = transform.position + Vector3.up;
            // friend.GetComponent<FriendController>().IsAttracted = true;

        }
    }
}
