using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextReveal : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        GetComponent<TextMeshPro>().color -= Color.black;
    }

    private void OnEnable()
    {
        GameManager.CheckPointReset += HideText;
    
    }
    
    private void OnDisable()
    {
        GameManager.CheckPointReset -= HideText;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("holder"))
        {
            _animator.SetTrigger("reveal");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("holder"))
        {
            _animator.SetTrigger("hide");
        }
        
    }
    
    public void HideText()
    {
     
        GetComponent<TextMeshPro>().color -= Color.black;
    }
    
}