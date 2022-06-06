using Script;
using UnityEngine;
using UnityEngine.AI;

public class AttractorEnemy : MonoBehaviour
{
    private Animator _animator;
    private bool isPulling;
    private FriendController _friendController;
    private float _obstacleRadius;
    private NavMeshObstacle _obstacle;
    [SerializeField] private GameObject friend;
    [SerializeField] private float forceIntensity;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _animator = GetComponent<Animator>();
        _obstacle = GetComponent<NavMeshObstacle>();
        _obstacleRadius = _obstacle.radius;
        _friendController = friend.GetComponent<FriendController>();
    }


    private void SetObstacleRadius()
    {
        if (_friendController.friendState == FriendController.FriendState.Returning)
        {
            _obstacle.radius = _obstacleRadius;
            return;
        }

        float distanceLocal = transform.InverseTransformPoint(friend.transform.position).magnitude;
        _obstacle.radius = Mathf.Min((distanceLocal) / 2, _obstacleRadius);
        if (_obstacle.radius < 0.5f)
        {
            _obstacle.radius = 0;
        }
    }

    private void FixedUpdate()
    {
        SetObstacleRadius();

        if (isPulling && _friendController.friendState == FriendController.FriendState.Idle)
        {
            AttractFriend();
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            isPulling = true;
            _animator.SetBool("IsPulling", true);
            col.gameObject.GetComponent<FriendController>().IsAttracted = true;
            _audioManager.Play("blackHole");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            isPulling = false;
            _animator.SetBool("IsPulling", false);
            other.gameObject.GetComponent<FriendController>().IsAttracted = false;
            _audioManager.Stop("blackHole");
        }
    }

    private void AttractFriend()
    {
        var position = transform.position;
        var friendposition = friend.transform.position;
        float distance = (position - friendposition).magnitude;

        Vector2 force = (position - friendposition).normalized / distance * forceIntensity;
        force = Vector2.ClampMagnitude(force, 2000);
        friend.GetComponent<Rigidbody2D>().AddForce(force);
        if (distance < 1)
        {
            isPulling = false;
            _audioManager.Stop("blackHole");
            if (GameManager.LastCheckPoint.gameObject.CompareTag("LastCheckPoint"))
            {
                GameManager.ChangeToNextLevelFlag = true;
                GameManager.InvokeFadeOut();
            }
            else
            {
                GameManager.ChangeToNextLevelFlag = false;
                GameManager.InvokeFadeOut();
            }
        }
    }
}