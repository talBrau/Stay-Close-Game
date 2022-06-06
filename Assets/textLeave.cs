using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textLeave : MonoBehaviour
{
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("reveal");
        var position = GetComponent<RectTransform>().position;
        position.z = 0;
    }
    private void OnEnable()
    {
        GameManager.CheckPointReset += DestroyLeaveText;
    
    }
    
    private void OnDisable()
    {
        GameManager.CheckPointReset -= DestroyLeaveText;
    }

    public void DestroyLeaveText()
    {
        Destroy(gameObject);
    }
}
