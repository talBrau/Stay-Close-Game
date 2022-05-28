using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private int statingDirection;

    private Vector3 initPosition;
    private bool _move = false;
    private int _direction; // platform moving up =1 , platform moving down = 0 

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetPlatform;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetPlatform;
    }

    private void Start()
    {
        initPosition = transform.position;
        _direction = statingDirection;
    }

    public void MovePlatform()
    {
        _move = true;
    }

    public void FreezePlatform()
    {
        _move = false;
    }

    private void FixedUpdate()
    {
        if (!_move) return;

        transform.position = _direction == 1
            ? transform.position + Vector3.up * speed
            : transform.position + Vector3.down * speed;

        if (transform.position.y > maxHeight || transform.position.y < minHeight)
        {
            //swap direction
            _direction = Mathf.Abs(_direction - 1);
        }
    }

    private void ResetPlatform()
    {
        transform.position = initPosition;
        _move = false;
        _direction = statingDirection;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.transform.SetParent(null);
    }
}