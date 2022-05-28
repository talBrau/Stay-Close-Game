using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReveal : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("reveal");
            print("reve");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("hide");
            print("hide");

        }
    }
}