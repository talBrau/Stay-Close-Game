using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class FriendAgentScript : MonoBehaviour
{

    #region Fields

    [SerializeField] private GameObject player;
    [SerializeField] private float agentSpeed;
    private friendController _friendController;
    private Transform target;
    private NavMeshAgent _agent;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _friendController = GetComponent<friendController>();
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
                    _agent.SetDestination(_friendController.spots[0].gameObject.transform.position);
                    _agent.acceleration = agentSpeed;
                }
            }
            else
            {
                _agent.SetDestination(player.transform.position);
                _friendController.HasTarget = false;
            }
        }
    }

    #endregion



}
