using System;
using UnityEngine;

public class FallingMarble : MonoBehaviour
{
    private Vector3 initPoistion;
    private Rigidbody2D rb;
    private SpriteRenderer _spriteRenderer;
    public bool fallen = false;
    private void Start()
    {
        initPoistion = transform.position;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb.simulated = false;
        fallen = false;
        _spriteRenderer.color -= Color.black;

    }

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetPosition;
    }

    private void OnDisable()
    {
        GameManager.CheckPointReset -= ResetPosition;
    }

    public void ActivateTrap()
    {
        _spriteRenderer.color += Color.black;
        rb.simulated = true;
        // rb.velocity = Vector2.right;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("NoJumpGround")||col.gameObject.CompareTag("Ground"))
        {
            fallen = true;
        }
        if (col.gameObject.CompareTag("Player") && !fallen)
        {
            GameManager.CheckPointInvoke();
        }
    }

    private void ResetPosition()
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        transform.position = initPoistion;
        _spriteRenderer.color -= Color.black;

    }
}
