using System;
using UnityEngine;
using UnityEngine.Serialization;


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
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float jumpGraceTime = 0.2f;

    #endregion

    #region Fields

    private float jumpTimer;
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
        audioManager = FindObjectOfType<AudioManager>();
        _holder = GameObject.Find("Holder");
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioManager.Play("Bg_Music");
        audioManager.Play("Ambience");
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle") ||
            col.gameObject.CompareTag("Moving Platform"))
        {
            _canJump = true;
            jumpTimer = 0;
            _animator.SetBool("OnGround", true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Moving Platform") || other.gameObject.CompareTag("Ground"))
        {
            jumpTimer = jumpGraceTime;
            print("Grace");
            _animator.SetBool("OnGround", false);
        }
    }

    private void Update()
    {
        if (!(jumpTimer > 0)) return;
        jumpTimer -= Time.deltaTime;
        if (!(jumpTimer <= 0)) return;
        print("cantJump");
        _canJump = false;
        jumpTimer = 0;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(friend.transform.position, transform.position) > distanceToFreeze)
        {
            _animator.SetBool("IsFreeze", true);
            _freeze = true;
            return;
        }

        _animator.SetBool("IsFreeze", false);
        if (_freeze)
        {
            Invoke("Unfreeze", 0.4f);
            audioManager.Stop("walk");
            return;
        }

        if (_horizontalDirection != 0)
        {
            _rb.velocity = new Vector2(_horizontalDirection * moveSpeed, _rb.velocity.y);
            FlipPlayer(_horizontalDirection < 0);
            _animator.SetBool("IsMoving", true);
            audioManager.Play("walk");
        }
        else
        {
            _animator.SetBool("IsMoving", false);
            audioManager.Stop("walk");
        }
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
            _animator.SetTrigger("Jump");
            audioManager.Stop("walk");
            audioManager.Play("jump");
            _canJump = false;
        }
    }

    public void ShortJump()
    {
        audioManager.Play("jump");
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
        {
            transform.position = GameManager.LastCheckPoint.transform.position;
            audioManager.Stop("walk");
        }
    }

    public void Unfreeze()
    {
        _freeze = false;
    }

    #endregion
}