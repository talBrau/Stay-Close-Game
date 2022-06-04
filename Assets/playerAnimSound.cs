using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimSound : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void playLandSound()
    {
        _audioManager.Play("land");
    }
}
