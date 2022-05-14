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

        #endregion

        #region Feilds

        public List<Spot> spots;
        private Vector2 _velocity = Vector2.zero;
        private FriendAgentScript _friendAgent;
        private int _currentSpotInd;

        public Spot CurrentSpot { get; private set; }

        public Spot OnSpot { get; set; }


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

        #region MonoBehaviour

        void Start()
        {
            friendState = FriendState.Idle;
            spots = new List<Spot>();
            _friendAgent = GetComponent<FriendAgentScript>();
            _currentSpotInd = -1;
        }
    
        void Update()
        {
            if (friendState == FriendState.Idle)
            {
                MoveAroundPlayer();
            }

            if (friendState == FriendState.Travelling)
            {
                if (_friendAgent._agent.remainingDistance < 1)
                {
                    _friendAgent._agent.autoBraking = true;
                }

                // print("travel");
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
            }

            if (friendState == FriendState.AtTarget)
            {
                // print("atTarget");
            }
        }

        #endregion

        #region Methods

        private void UpdateStateUponArrival()
        {
            _friendAgent._agent.autoBraking = true;
            /*GetComponent<Collider2D>().isTrigger = true;*/
            if (friendState == FriendState.Travelling)
            {
                friendState = FriendState.AtTarget;
                _currentSpotInd = -1;
            }

            if (friendState == FriendState.Returning)
            {
                friendState = FriendState.Idle;
                _friendAgent.SetNoDestination();
                _currentSpotInd = -1;
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
                friendState = FriendState.Idle;
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
        #endregion
    }
}