using System;
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
    [SerializeField] private float distanceToFreeze;

    #endregion

    #region Fields

    private GameObject _curObstacle;
    private bool _freeze;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public bool Freeze
    {
        get => _freeze;
        set => _freeze = value;
    }

    private bool _canJump;
    public bool CanJump => _canJump;

    private bool _isLifting;
    private bool _canLift;
    private float _horizontalDirection;
    private Rigidbody2D _rb;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetPlayer;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetPlayer;
    }
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle"))
        {
            _canJump = true;
            _animator.SetBool("OnGround", true);
        }

        if (col.gameObject.CompareTag("Obstacle"))
        {
            _canLift = true;
            _curObstacle = col.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _canJump = false;
            _animator.SetBool("OnGround", false);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            _canLift = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Moving Platform"))
        {
            _canJump = true;
            _animator.SetBool("OnGround", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Moving Platform"))
        {
            _canJump = false;
            _animator.SetBool("OnGround", false);
        }
    }

    private void FixedUpdate()
    {
        if (_freeze ||
            Vector2.Distance(friend.transform.position, transform.position) > distanceToFreeze)
            return;

        if (_horizontalDirection != 0)
        {
            _rb.velocity = new Vector2(_horizontalDirection * moveSpeed, _rb.velocity.y);
            _spriteRenderer.flipX = _horizontalDirection > 0;
            _animator.SetBool("IsMoving",true);
        }
        else 
            _animator.SetBool("IsMoving",false);
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
            _animator.SetTrigger("Jump");
        }
    }

    public void ShortJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * shortJumpReduce);
    }

    public void MagnetToFriend()
    {
        var forceVec = (friend.transform.position - gameObject.transform.position).normalized;
        _rb.AddForce(forceVec * magnetForce);
    }

    private void ResetPlayer()
    {
        
        transform.position = GameManager.LastCheckPoint.transform.position;
    }
    
    
    #endregion
}