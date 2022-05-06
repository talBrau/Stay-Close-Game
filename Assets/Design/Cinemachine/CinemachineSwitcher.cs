using System;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject CMSwitcher;

    #endregion

    #region Fields

    private bool _onDefaultCamera = true;
    private Animator _animator;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _animator = CMSwitcher.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
            SwitchCameras();
    }

    #endregion

    #region Methods

    private void SwitchCameras()
    {
        _onDefaultCamera = !_onDefaultCamera;
        if (_onDefaultCamera)
           _animator.Play("Default Camera");
        else
            _animator.Play("Zoom Camera");
        print("Camera Switch!");
    }

    #endregion
    
}
