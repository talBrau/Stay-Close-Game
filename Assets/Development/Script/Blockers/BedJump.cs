using System;
using UnityEngine;

public class BedJump : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private void StopPlayer()
    {
        player.GetComponent<PlayerManager>().Freeze = true;
    }

    public void ResumePlayer()
    {
        player.GetComponent<PlayerManager>().Freeze = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            StopPlayer();
    }
}
