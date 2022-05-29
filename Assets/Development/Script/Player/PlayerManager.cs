using System;
using UnityEngine;


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

    private bool _freeze;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GameObject _holder;

    public bool Freeze
    {
        get => _freeze;
        set => _freeze = value;
    }

    private bool _canJump;
    public bool CanJump => _canJump;

 
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
        _holder = GameObject.Find("Holder");
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle"))
        {
            _canJump = true;
            _animator.SetBool("OnGround", true);
        }
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _canJump = false;
            _animator.SetBool("OnGround", false);
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Moving Platform") || col.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
            _animator.SetBool("OnGround", true);
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Moving Platform")|| other.gameObject.CompareTag("Ground"))
        {
            _canJump = false;
            _animator.SetBool("OnGround", false);
        }
    
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(friend.transform.position, transform.position) > distanceToFreeze)
        {
            _animator.SetBool("IsFreeze",true);
            _freeze = true;
            return;
        }

        _animator.SetBool("IsFreeze",false);
        if (_freeze)
        {
            Invoke("Unfreeze",0.4f);
            return;
        }
        
        if (_horizontalDirection != 0)
        {
            _rb.velocity = new Vector2(_horizontalDirection * moveSpeed, _rb.velocity.y);
            FlipPlayer(_horizontalDirection < 0);
            _animator.SetBool("IsMoving",true);
        }
        else
            _animator.SetBool("IsMoving",false);
    }

    #endregion

    #region Methods

    public void SetFreezeDistance(float val) => distanceToFreeze = val;
    
    
    private void FlipPlayer(bool left)
    {
        if (left)
        {
            var rotation = _holder.transform.rotation;
            rotation.y = 180;
            _holder.transform.rotation = rotation;
        }
        else
        {
            var rotation = _holder.transform.rotation;
            rotation.y = 0;
            _holder.transform.rotation = rotation;
        }
    }
    

    public void Move(float input)
    {
        _horizontalDirection = input;
    }

    public void Jump()
    {
        if (_canJump && !_freeze)
        {
            _rb.AddForce(Vector2.up * jumpHeight);
            /*_rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);*/
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
        if (GameManager.LastCheckPoint) 
            transform.position = GameManager.LastCheckPoint.transform.position;
    }

    public void Unfreeze()
    {
        _freeze = false;
    }
    
    #endregion
}