using System;
using UnityEngine;

public class FallingMarble : MonoBehaviour
{
    private Vector3 initPoistion;
    private Rigidbody2D rb;
    private void Start()
    {
        initPoistion = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    public void ActivateTrap()
    {
        rb.simulated = true;
        rb.velocity = Vector2.right;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Invoke("ResetPosition",0.25f);
            GameManager.CheckPointInvoke();
        }
    }

    private void ResetPosition()
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        transform.position = initPoistion;
    }
}
