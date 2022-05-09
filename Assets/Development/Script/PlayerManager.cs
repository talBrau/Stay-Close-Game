using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject friend;
    [SerializeField] private float magnetForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float shortJumpReduce;
    [SerializeField] private GameObject obstacleParent;

    #endregion

    #region Fields

    private GameObject _curObstacle;
    
    private bool _canJump;
    public bool CanJump => _canJump;
    
    private bool _isLifting;
    private bool _canLift;
    private float _horizontalDirection;
    private Rigidbody2D _rb;
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle"))
            _canJump = true;
        if (col.gameObject.CompareTag("Obstacle"))
        {
            _canLift = true;
            _curObstacle = col.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _canJump = false;
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _canLift = false;
        }
    }

    private void FixedUpdate()
    {
        if (_horizontalDirection != 0)
            _rb.velocity = new Vector2(_horizontalDirection * moveSpeed, _rb.velocity.y);
    }
    #endregion
    
    #region Methods

    public void MoveObstacle()
    {
       
        if (_isLifting)
        {
            _curObstacle.transform.SetParent(obstacleParent.transform);
            _curObstacle.GetComponent<Rigidbody2D>().simulated = true;
            _curObstacle = null;
            _canLift = false;
            _isLifting = false;

        }
        else
        {
            if (_canLift)
            {
                _curObstacle.transform.SetParent(gameObject.transform);
                _curObstacle.GetComponent<Rigidbody2D>().simulated = false;
                _isLifting = true;
            }
            
            
        }

    }

    public void Move(float input)
    {
        _horizontalDirection = input;
    }

    public void Jump()
    {
        if (_canJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
        }
    }

    public void ShortJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * shortJumpReduce);
    }

    public void MagnetToFriend()
    {
        var forceVec = (friend.transform.position - gameObject.transform.position).normalized;
        print(forceVec);
        _rb.AddForce(forceVec * magnetForce);
    }

    #endregion
}