using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed;
    
    #endregion

    #region Fields

    private int _waypointsArrayIndex;
    private List<Vector3> _waypointsPositions;
    private bool _movingFlag;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _waypointsPositions = new List<Vector3>();
        foreach (var waypoint in waypoints)
        {
            _waypointsPositions.Add(waypoint.transform.position);
        }
    }

    private void OnEnable()
    {
        _waypointsArrayIndex = 0;
    }

    private void Update()
    {
        if (!_movingFlag)
            return;
        Move();
    }

    #endregion

    #region Methods

    private void Move()
    {
        if (waypoints.Length == 0)
            return;
        
        if (transform.position == _waypointsPositions[_waypointsArrayIndex])
        {
            _waypointsArrayIndex++;
            if (_waypointsArrayIndex == waypoints.Length)
                _waypointsArrayIndex = 0;
        }
        
        transform.position = Vector2.MoveTowards(transform.position,
            _waypointsPositions[_waypointsArrayIndex], speed * Time.deltaTime);
    }

    public void ActivateMovement()
    {
        _movingFlag = true;
    }

    public void DisableMovement()
    {
        _movingFlag = false;
    }
    #endregion



}
