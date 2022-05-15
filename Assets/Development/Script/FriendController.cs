using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


namespace Script
{
    public class FriendController : MonoBehaviour
    {
        #region SerielizedFields

        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float maxSpeed = 100;
        [SerializeField] private float borderOffset = 1.5f;
        [SerializeField] private GameObject border;
        [SerializeField] private float distanceToAutoBreak = 3;
        [SerializeField] private float forceIntensity;

        #endregion

        #region Feilds

        public List<Spot> spots;
        private Vector2 _velocity = Vector2.zero;
        private FriendAgentScript _friendAgent;
        private int _currentSpotInd;
        public bool IsAttracted { get; set; }
        public Spot CurrentSpot { get; private set; }

        public Spot OnSpot { get; set; }


        public enum FriendState
        {
            Idle,
            Travelling,
            Returning,
            AtTarget,
        }

        public FriendState friendState;

        private bool _hasTarget;

        public bool HasTarget
        {
            get => _hasTarget;
            set => _hasTarget = value;
        }

        #endregion

        #region MonoBehaviour

        void Start()
        {
            friendState = FriendState.Idle;
            spots = new List<Spot>();
            _friendAgent = GetComponent<FriendAgentScript>();
            _currentSpotInd = -1;
            IsAttracted = false;
        }

        void FixedUpdate()
        {
            if (Vector3.Distance(gameObject.transform.position, border.gameObject.transform.position) > 15 &&
                friendState == FriendState.Idle && !IsAttracted)
            {
                print("far"); 
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _friendAgent.ReturnFriend();
            }

            if (friendState == FriendState.Idle &&
                Vector3.Distance(gameObject.transform.position, border.gameObject.transform.position) <= 15)
            {
                MoveAroundPlayer();
            }


            if (friendState == FriendState.Travelling)
            {
                if (_friendAgent._agent.remainingDistance < 1)
                {
                    _friendAgent._agent.autoBraking = true;
                    UpdateStateUponArrival();
                }
            }


            if (friendState == FriendState.Returning)
            {
                _friendAgent.setReturnDest(border.transform.position);
                if (Vector2.Distance(transform.position, border.transform.position) < distanceToAutoBreak)
                {
                    _friendAgent._agent.autoBraking = true;
                    _friendAgent.SetNoDestination();
                    UpdateStateUponArrival();
                }
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

                // _friendAgent.SetNoDestination();

                _currentSpotInd = -1;
            }

            if (friendState == FriendState.Returning)
            {

                friendState = FriendState.Idle;
                _friendAgent.SetNoDestination();
                _currentSpotInd = -1;
            }

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }


        private void MoveAroundPlayer()
        {
            if (IsAttracted)
            {
                return;
            }
            _friendAgent.SetNoDestination();

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

            // if (IsAttracted)
            // {
            //     // var borderposition = border.transform.position;
            //     float distance = (position - targetPos).magnitude;
            //     Vector2 force = (border.transform.position - position).normalized * forceIntensity;
            //     force = Vector2.ClampMagnitude(force, 2000);
            //     GetComponent<Rigidbody2D>().AddForce(force);
            //     return;
            // }

            transform.position = Vector2.SmoothDamp(position, targetPos,
                ref _velocity, smoothTime, maxSpeed);
            // transform.position = position;
        }

        #endregion

        #region PublicFunctions

        public void EnteredSpot(Spot spot)
        {
            spots.Add(spot);
            if (spots.Count == 1)
            {
                CurrentSpot = spots[0];
                _currentSpotInd = 0;
                CurrentSpot.HighlightSpot();
            }
            else
            {
                spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
                CurrentSpot.UnHighlightSpot();
                CurrentSpot = spots[0];
                CurrentSpot.HighlightSpot();
            }
        }

        public void ExitSpot(Spot spot)
        {
            spots.Remove(spot);
            if (spots.Count == 0)
            {
                CurrentSpot.UnHighlightSpot();
                CurrentSpot = null;
            }
            else
            {
                spots = spots.OrderBy(s => (s.transform.position - transform.position).magnitude).ToList();
                if (spot == CurrentSpot)
                {
                    _currentSpotInd = 0;
                    CurrentSpot.UnHighlightSpot();
                }
                else
                {
                    for (int i = 0; i < spots.Count; i++)
                    {
                        if (spots[i] == CurrentSpot)
                            _currentSpotInd = i;
                    }
                }

                CurrentSpot = spots[_currentSpotInd];
                CurrentSpot.HighlightSpot();
            }
        }

        public void ChangeSpotTarget(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (spots.Count > 1 && (friendState == FriendState.Idle || friendState == FriendState.AtTarget))
            {
                CurrentSpot.UnHighlightSpot();
                _currentSpotInd++;
                if (_currentSpotInd == spots.Count)
                    _currentSpotInd = 0;
                CurrentSpot = spots[_currentSpotInd];
                CurrentSpot.HighlightSpot();
            }
        }

        public void ActivateSpotEvent(Spot spot)
        {
            spot.spotEvent?.Invoke();
        }

        public void ActivateSpotLeaveEvent(Spot spot)
        {
            spot.spotLeaveEvent?.Invoke();
        }
        #endregion
    }
}