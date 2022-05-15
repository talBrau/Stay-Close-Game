using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.AI;

public class AttractorEnemy : MonoBehaviour
{
    private bool isPulling;
    private FriendController _friendController;
    private float _obstacleRadius;
    private NavMeshObstacle _obstacle;
    [SerializeField] private GameObject friend;
    [SerializeField] private float forceIntensity;

    private void Start()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        _obstacleRadius = _obstacle.radius;
        _friendController = friend.GetComponent<FriendController>();
    }
    

    private void SetObstacleRadius()
    {
        if (_friendController.friendState == FriendController.FriendState.Returning)
        {
            _obstacle.radius = _obstacleRadius;
            return;
        }
        
        float distanceLocal = transform.InverseTransformPoint(friend.transform.position).magnitude;
        _obstacle.radius = Mathf.Min((distanceLocal) / 2, _obstacleRadius);
        if (_obstacle.radius < _obstacleRadius)
        {
            _obstacle.radius = 0;
        }
        print(distanceLocal);

    }

    private void FixedUpdate()
    {
        SetObstacleRadius();

        if (isPulling && _friendController.friendState == FriendController.FriendState.Idle)
        {
            attractFriend();
        }
       
    }

  

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            isPulling = true;
            col.gameObject.GetComponent<FriendController>().IsAttracted = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            isPulling = false;
            other.gameObject.GetComponent<FriendController>().IsAttracted = false;
        }
    }

    private void attractFriend()
    {
        var position = transform.position;
        var friendposition = friend.transform.position;
        float distance = (position - friendposition).magnitude;
        // float curIntensity  =
        //     _friendController.friendState == FriendController.FriendState.Travelling
        //         ? forceIntensity / 5
        //         : forceIntensity;

        Vector2 force = (position - friendposition).normalized / distance * forceIntensity;
        force = Vector2.ClampMagnitude(force, 2000);
        friend.GetComponent<Rigidbody2D>().AddForce(force);
        if (distance < 0.8)
        {
            print("endgame");
            GameObject.Find("SceneManager").GetComponent<SceneManager>().ChangeLevel(false);
        }
    }
}