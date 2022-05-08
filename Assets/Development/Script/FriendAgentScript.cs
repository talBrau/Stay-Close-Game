using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class FriendAgentScript : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject player;
    [SerializeField] private float agentSpeed;
    [SerializeField] private GameObject friendBorder;
    private FriendController _friendController;
    private Transform _target;
    private NavMeshAgent _agent;

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
                    _agent.acceleration = agentSpeed;
                    _friendController.friendState = FriendController.FriendState.Travelling;
                    _friendController.spots[spot].UnHighlightSpot();
                }
            }
            else
            {
                //TODO: set destination to moving target upon return to player
                _agent.SetDestination(friendBorder.transform.position);
                _friendController.HasTarget = false;
                _friendController.friendState = FriendController.FriendState.Returning;
            }
        }
    }

    public bool arrivedOnTarget()
    {
        return _agent.remainingDistance < 1;
    }

    public void SetNoDestination()
    {
        _agent.ResetPath();
    }

    #endregion
}