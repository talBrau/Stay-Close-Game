using System;
using UnityEngine;

public class ChaserScript : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject player;
    [SerializeField] private float distance = 3;
    [SerializeField] private float speed;

    #endregion
    
    #region Fields

    private Vector3 _initialPosition;
    private Animator _animator;
    private AudioManager _audioManager;
    private bool isAttacking;
    
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetEnemy;
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
        _animator.SetBool("Attacking",true);
        _audioManager.Play("monster");
        isAttacking = true;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetEnemy;
    }

    private void FixedUpdate()
    {
        if (!isAttacking) return;
        var curSpeed = speed;
        if (Vector2.Distance(transform.position, player.transform.position) > distance + 3f)
            curSpeed = speed + 2;
        else
            curSpeed = speed / 2f;
        transform.position = Vector3.MoveTowards(transform.position,
                player.transform.position, 
                curSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ResetEnemy();
            GameManager.CheckPointInvoke();
        }
    }
    
    #endregion

    #region Methods

    public void StopEnemy()
    {
        _animator.SetBool("Attacking",false);
        _audioManager.Stop("monster");
        isAttacking = false;
    }

    private void ResetEnemy()
    {
        transform.position = _initialPosition;
        gameObject.SetActive(false);
    }


    #endregion
    
}
