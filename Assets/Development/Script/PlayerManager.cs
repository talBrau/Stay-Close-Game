using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float shortJumpReduce;
    

    #endregion

    #region Fields

    private bool _canJump;
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
        if (col.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
            print(_canJump);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _canJump = false;
            print(_canJump);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontalDirection * moveSpeed,_rb.velocity.y);
    }

    #endregion

    #region Methods

    public void Move(float input)
    {
        _horizontalDirection = input;
    }
    
    public void Jump()
    {
        if (_canJump)
        {
            print("doJump");
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
        }
    }

    public void shortJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * shortJumpReduce);
    }

    #endregion
    
}
