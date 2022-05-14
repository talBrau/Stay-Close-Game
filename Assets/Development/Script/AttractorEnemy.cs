using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class AttractorEnemy : MonoBehaviour
{
    private bool isPulling;
    [SerializeField] private Rigidbody2D friendRB;

    private void FixedUpdate()
    {
        if (isPulling)
        {
            attractFriend();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            isPulling = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            isPulling = false;
            friendRB.AddForce(Vector2.zero);
        }
    }

    private void attractFriend()
    {
        if (friendRB.gameObject.GetComponent<FriendController>().friendState == FriendController.FriendState.AtTarget)
        {
            // friendRB.AddForce(Vector2.zero);
            return;
        }
        Rigidbody2D myRb = GetComponent<Rigidbody2D>();
        float distance = (myRb.position - friendRB.position).magnitude;
        float forceMag = myRb.mass * friendRB.mass / Mathf.Pow(distance, 2);
        Vector2 force = (myRb.position - friendRB.position).normalized * forceMag;
        friendRB.AddForce(force);
    }

    // private void OnCollisionEnter2D(Collision2D col)
    // {
    //     if (col.gameObject.CompareTag("friend"))
    //     {
    //         print("endgame");
    //         GameObject.Find("SceneManager").GetComponent<SceneManager>().ChangeLevel(false);
    //     }
    // }
}