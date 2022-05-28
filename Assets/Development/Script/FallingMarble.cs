using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.LWRP;

public class FallingMarble : MonoBehaviour
{
    [SerializeField] private Vector2 startingVel = Vector2.zero;
    private Vector3 initPoistion;
    private Rigidbody2D rb;
    private SpriteRenderer _spriteRenderer;
    public bool fallen;
    public UnityEvent WhenLeaveGround; 
    private void Start()
    {
        initPoistion = transform.position;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb.simulated = false;
        fallen = false;
        AlphaChange(true);
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
        AlphaChange(false);
        rb.simulated = true;
        rb.velocity = startingVel;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("NoJumpGround") || col.gameObject.CompareTag("Ground"))
        {
            fallen = true;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "FallingMarble" || !fallen)
                GameManager.CheckPointInvoke();
        }
    }
    
    private void ResetPosition()
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        transform.position = initPoistion;
        AlphaChange(true);
    }

    private void AlphaChange(bool turnTransparent)
    {
        var curColor = _spriteRenderer.color;
        curColor.a = turnTransparent ? 0 : 1;
        _spriteRenderer.color = curColor;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DeadZone"))
            WhenLeaveGround.Invoke();
    }
}
