using Script;
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
            if ((_friendController.friendState == FriendController.FriendState.AtTarget && 
                 _friendController.CurrentSpot == _friendController.OnSpot ||
                 
                 _friendController.friendState == FriendController.FriendState.Travelling))
            {
                ReturnFriend();
            }

            else // at player or at target and chose spot
            {
                if (_friendController.CurrentSpot)
                {
                    _agent.SetDestination(_friendController.CurrentSpot.gameObject.transform.position);
                    _agent.stoppingDistance = 0;
                    _agent.acceleration = agentAcceleration;
                    _agent.speed = agentSpeed;
                    _friendController.friendState = FriendController.FriendState.Travelling;
                    _agent.autoBraking = false;
                    // GetComponent<Collider2D>().isTrigger = false;
                }
            }
        }
    }

    public void ReturnFriend()
    {
        _agent.SetDestination(friendBorder.transform.position);
        _agent.acceleration = agentAcceleration;
        _agent.speed = agentSpeed;
        _agent.stoppingDistance = agentStoppingDist;
        GetComponent<Rigidbody2D>().simulated = true;
        _friendController.friendState = FriendController.FriendState.Returning;
        _agent.autoBraking = false;
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