using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private bool _move = false;
    private int _direction = 1; // platform moving up =1 , platform moving down = 0 


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
}