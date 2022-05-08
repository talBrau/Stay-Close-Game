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
            if (!_friendController.HasTarget)
            {
                if (_friendController.spots.Count > 0)
                {
                    _friendController.HasTarget = true;
                    var spot = _friendController.SpotChosen;
                    _agent.SetDestination(_friendController.spots[spot].gameObject.transform.position);
                    _agent.stoppingDistance = 0;
                    _agent.acceleration = agentAcceleration;
                    _agent.speed = agentSpeed;
                    _friendController.friendState = FriendController.FriendState.Travelling;
                    _agent.autoBraking = false;
                    _friendController.spots[spot].UnHighlightSpot();
                }
            }
            else
            {
                _agent.SetDestination(friendBorder.transform.position);
                _agent.acceleration = agentAcceleration;
                _agent.speed = agentSpeed;
                _agent.stoppingDistance = agentStoppingDist;
                _friendController.HasTarget = false;
                _friendController.friendState = FriendController.FriendState.Returning;
                _agent.autoBraking = false;

            }
        }
    }
    
    
    public bool arrivedOnTarget()
    {
        return _agent.remainingDistance < .5f;
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