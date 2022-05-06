using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class FriendController : MonoBehaviour
{
    [ReadOnly] [SerializeField] private List<Spot> spots;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float borderOffset;
    [SerializeField] private GameObject border;
    private Vector2 _velocity = Vector2.zero;
    private int _spotChosen;

    private enum FriendState
    {
        Idle,
        Looking,
        Targeted
    }

    private FriendState _friendState;

    void Start()
    {
        _friendState = FriendState.Idle;
        spots = new List<Spot>();
    }

    void Update()
    {
        if (_friendState == FriendState.Idle || _friendState == FriendState.Looking)
        {
            MoveAroundPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Z) && spots.Count > 0)
        {
            HighlightSpots();
        }
    }

    private void HighlightSpots()
    {
        //sort by distance
        spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();

        // start choosing spot
        if (_friendState == FriendState.Idle)
        {
            _friendState = FriendState.Looking;
            _spotChosen = 0;
            spots[_spotChosen].HighlightSpot();
        }

        // already looking, highlight next target and un-highlight current.
        else
        {
            if (_friendState == FriendState.Looking)
            {
                spots[_spotChosen].UnHighlightSpot();
                //been through all, go back to idle
                if (_spotChosen == spots.Count - 1)
                {
                    _friendState = FriendState.Idle;
                }
                else
                {
                    spots[++_spotChosen].HighlightSpot();
                }
            }
        }
    }

    public void EnteredSpot(Spot spot)
    {
        spots.Add(spot);
        spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
    }

    public void ExitSpot(Spot spot)
    {
        spots.Remove(spot);
        spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
        if (spots.Count == 0)
        {
            _friendState = FriendState.Idle;
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
        var randomCirclePoint = new Vector3(randX, randY, 0);

        //calc target position and move 
        var targetPos = border.transform.TransformPoint(randomCirclePoint * borderOffset);
        position = Vector2.SmoothDamp(position, targetPos,
            ref _velocity, smoothTime, maxSpeed);
        transform.position = position;
    }
}