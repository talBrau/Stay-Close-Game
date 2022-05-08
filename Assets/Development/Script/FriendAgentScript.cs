using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class FriendAgentScript : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject player;
    [SerializeField] private float agentAcceleration;
    [SerializeField] private float agentSpeed;
    [SerializeField] private float agentStoppingDist;
    [SerializeField] private GameObject friendBorder;
    private FriendController _friendController;
    private Transform _target;
    public NavMeshAgent _agent;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _friendController = GetComponent<FriendController>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    #endregion

    #region Methods

    public void GoToSpot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //at target/travelling, none chosen -> return to player
            if ((_friendController.friendState == FriendController.FriendState.AtTarget ||
                 _friendController.friendState == FriendController.FriendState.Travelling) &&
                _friendController.SpotChosen == -1)
            {
                _agent.SetDestination(friendBorder.transform.position);
                _agent.acceleration = agentAcceleration;
                _agent.speed = agentSpeed;
                _agent.stoppingDistance = agentStoppingDist;
                // _friendController.HasTarget = false;
                _friendController.friendState = FriendController.FriendState.Returning;
                _agent.autoBraking = false;
            }

            else // at player or at target and chose spot
            {
                if (_friendController.spots.Count > 0)
                {
                    var spot = _friendController.SpotChosen;
                    // _friendController.HasTarget = true;
                    if (spot == -1) // no target selected -> go to first
                    {
                        spot++;
                    }

                    _agent.SetDestination(_friendController.spots[spot].gameObject.transform.position);
                    _agent.stoppingDistance = 0;
                    _agent.acceleration = agentAcceleration;
                    _agent.speed = agentSpeed;
                    _friendController.friendState = FriendController.FriendState.Travelling;
                    _agent.autoBraking = false;
                    _friendController.spots[spot].UnHighlightSpot();
                }
            }
        }
    }


    public void SetNoDestination()
    {
        _agent.ResetPath();
    }

    public void setReturnDest(Vector2 pos)
    {
        _agent.SetDestination(pos);
    }

    #endregion
}