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
    [SerializeField] private float distance = 3;
    [SerializeField] private float speed;
    [SerializeField] private bool isStatic;

    #endregion
    
    #region Fields

    private Vector3 _initialPosition;
    private State _state;
    private GameObject _targetObj;
    private Vector3 _target;
    private Animator _animator;
    private AudioManager _audioManager;
    
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _initialPosition = transform.position;
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
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
        if (_state == State.Idle || isStatic)
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
            GameManager.ChangeToNextLevelFlag = false;
            player.GetComponent<PlayerManager>().Freeze = true;
            GameManager.InvokeFadeOut();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendRaduis") && friend.friendState != FriendController.FriendState.AtTarget)
            AwakeEnemy();
        else if (col.gameObject.CompareTag("FriendRaduis") && 
                 Vector2.Distance(col.gameObject.transform.position,
                        gameObject.transform.position) > distance)
            IdleEnemy();
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FriendRaduis") && !isStatic)
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
        _audioManager.PlayDelay("monster");
    }

    public void IdleEnemy()
    {
        _animator.SetBool("Attacking",false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _state = State.Idle;
        _targetObj = null;
        _audioManager.Stop("monster");

    }

    private void ResetEnemy()
    {
        transform.position = _initialPosition;
        IdleEnemy();
        _animator.Play("Monster Idle");
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    public void VanishMonster()
    {
        _animator.Play("monsters gone");
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _state = State.Idle;
        _targetObj = null;
        _audioManager.Stop("monster");
    }
    #endregion
    
}
