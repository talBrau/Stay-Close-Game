using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class FriendController : MonoBehaviour
{
    #region SerielizedFields

    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float borderOffset = 1.5f;
    [SerializeField] private GameObject border;
    [SerializeField] private float distanceToAutoBreak = 1;

    #endregion

    #region Feilds

    public List<Spot> spots;
    private Vector2 _velocity = Vector2.zero;
    private FriendAgentScript _friendAgent;
    private int _spotChosen;

    public int SpotChosen
    {
        get => _spotChosen;
        set => _spotChosen = value;
    }

    public enum FriendState
    {
        Idle,
        Travelling,
        Returning,
        AtTarget
    }

    public FriendState friendState;

    private bool _hasTarget;

    public bool HasTarget
    {
        get => _hasTarget;
        set => _hasTarget = value;
    }

    #endregion

    #region Mono

    void Start()
    {
        friendState = FriendState.Idle;
        spots = new List<Spot>();
        _friendAgent = GetComponent<FriendAgentScript>();
        _spotChosen = -1;
    }

    void Update()
    {
        if (friendState == FriendState.Idle)
        {
            print("idle");
            MoveAroundPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Z) && spots.Count > 0 &&
            (friendState == FriendState.Idle || friendState == FriendState.AtTarget))
        {
            HighlightSpots();
        }

        if (friendState == FriendState.Travelling)
        {
            if (_friendAgent._agent.remainingDistance < distanceToAutoBreak)
            {
                _friendAgent._agent.autoBraking = true;
            }

            print("travel");
        }

        if (_friendAgent._agent.remainingDistance < .5f)
        {
            UpdateStateUponArrival();
        }


        if (friendState == FriendState.Returning)
        {
            _friendAgent.setReturnDest(border.transform.position);
            if (_friendAgent._agent.remainingDistance < distanceToAutoBreak)
            {
                _friendAgent._agent.autoBraking = true;
            }

            print("return");
        }

        if (friendState == FriendState.AtTarget)
        {
            print("atTarget");
        }
    }

    #endregion

    #region Methods

    private void UpdateStateUponArrival()
    {
        _friendAgent._agent.autoBraking = true;

        if (friendState == FriendState.Travelling)
        {
            friendState = FriendState.AtTarget;
            _spotChosen = -1;
        }

        if (friendState == FriendState.Returning)
        {
            friendState = FriendState.Idle;
            _friendAgent.SetNoDestination();
            _spotChosen = -1;
        }
    }

    private void HighlightSpots()
    {
        //sort by distance
        if (_spotChosen == -1)
        {
            spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
        }

        if (_spotChosen > -1) // already started choosing
        {
            spots[_spotChosen].UnHighlightSpot();
        }

        //been through all, go back to idle
        if (_spotChosen >= spots.Count - 1)
        {
            _spotChosen = -1;
        }
        else
        {
            spots[++_spotChosen].HighlightSpot();
        }
    }

    private void MoveAroundPlayer()
    {
        // var randomCirclePoint = Random.insideUnitSphere;

        //choose sign randomly
        var randSignX = Random.value > .5f ? 1 : -1;
        var randSignY = Random.value > .5f ? 1 : -1;

        //take perlin noise according to world pos and create random circle point
        var position = transform.position;
        var randX = Mathf.PerlinNoise(position.x, 0) * randSignX;
        var randY = Mathf.PerlinNoise(position.y, 0) * randSignY;
        var randomCirclePoint = new Vector3(randX, randY, 0).normalized;

        //calc target position and move 
        var targetPos = border.transform.TransformPoint(randomCirclePoint * borderOffset);
        transform.position = Vector2.SmoothDamp(position, targetPos,
            ref _velocity, smoothTime, maxSpeed);
        // transform.position = position;
    }

    #endregion

    #region PublicFunctions

    public void EnteredSpot(Spot spot)
    {
        spots.Add(spot);
        spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
    }

    public void ExitSpot(Spot spot)
    {
        spot.UnHighlightSpot();
        spots.Remove(spot);
        spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
        if (spots.Count == 0)
        {
            friendState = FriendState.Idle;
        }

        _spotChosen = -1;
    }

    #endregion
}