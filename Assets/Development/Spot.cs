using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            col.gameObject.GetComponent<friendController>().EnteredSpot(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            other.gameObject.GetComponent<friendController>().ExitSpot(this);
        }
    }
}
