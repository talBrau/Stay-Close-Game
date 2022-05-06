using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    #endregion

    #region Fields

    private bool _canJump;
    private Vector2 _direction;
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
        _rb.velocity = _direction * moveSpeed;
    }

    #endregion

    #region Methods

    public void Move(Vector2 input)
    {
        _direction = input;
    }
    
    public void Jump()
    {
        if (_canJump)
        {
            print("doJump");
            _rb.AddForce(Vector2.up * jumpHeight);
        }
    }

    #endregion
    
}
