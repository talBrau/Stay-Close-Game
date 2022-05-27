using System;
using Script;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    enum State
    {
        Idle, Attack
    }

    #region Inspector

    [SerializeField] private GameObject player;
    [SerializeField] private FriendController friend;

    [SerializeField] private float speed;

    #endregion
    
    #region Fields

    private Vector3 _initialPosition;
    private State _state;
    private GameObject _targetObj;
    private Vector3 _target;
    private Animator _animator;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _initialPosition = transform.position;
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        friend = GameObject.FindWithTag("friend").GetComponent<FriendController>();
        GameManager.CheckPointReset += ResetEnemy;
        _initialPosition = transform.position;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetEnemy;
    }

    private void FixedUpdate()
    {
        if (_state == State.Idle)
            return;
        
        if (_state == State.Attack)
        {
            if (friend.friendState == FriendController.FriendState.AtTarget)
            {
                IdleEnemy();
                return;
            }
            _target = _targetObj.transform.position;
        }
        
        transform.position = Vector3.MoveTowards(transform.position,
                                                        _target,
                                          speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ResetEnemy();
            GameManager.CheckPointInvoke();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendRaduis") && friend.friendState != FriendController.FriendState.AtTarget)
            AwakeEnemy();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FriendRaduis"))
            IdleEnemy();
    }

    #endregion

    #region Methods

    private void AwakeEnemy()
    {
        _animator.SetBool("Attacking",true);
        gameObject.layer = LayerMask.NameToLayer("Default");
        _state = State.Attack;
        _targetObj = player;
    }

    private void IdleEnemy()
    {
        _animator.SetBool("Attacking",false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _state = State.Idle;
        _targetObj = null;
    }

    private void ResetEnemy()
    {
        transform.position = _initialPosition;
        IdleEnemy();
        _animator.SetBool("Attacking",false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }


    #endregion
    
}
