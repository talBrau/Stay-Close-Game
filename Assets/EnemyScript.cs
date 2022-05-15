using System;
using Script;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    enum State
    {
        Idle, Attack, Cooldown, Retreat
    }

    #region Inspector

    [SerializeField] private GameObject player;
    [SerializeField] private FriendController friend;

    [SerializeField] private float speed;
    [SerializeField] private float cooldownTimer;

    #endregion
    
    #region Fields

    private State _state;
    private GameObject _targetObj;
    private Vector3 _target;
    private Vector3 _initialPosition;
    private float _timer;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        friend = GameObject.FindWithTag("friend").GetComponent<FriendController>();
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_state == State.Cooldown)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                RetreatEnemy();
        }
        
        if (_state == State.Retreat && Vector3.Distance(transform.position , _target) < 0.6f)
            IdleEnemy();
        print(Vector3.Distance(transform.position , _target));
    }

    private void FixedUpdate()
    {
        if (_state == State.Idle || _state == State.Cooldown)
            return;
        
        if (_state == State.Attack)
        {
            if (friend.friendState == FriendController.FriendState.AtTarget)
            {
                CooldownEnemy();
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
            GameObject.Find("SceneManager").GetComponent<SceneManager>().ChangeLevel(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendRaduis") && friend.friendState != FriendController.FriendState.AtTarget)
            AwakeEnemy();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FriendRaduis"))
            CooldownEnemy();
    }

    #endregion

    #region Methods

    private void AwakeEnemy()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.layer = LayerMask.NameToLayer("Default");
        _state = State.Attack;
        _targetObj = player;
    }

    private void CooldownEnemy()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        _state = State.Cooldown;
        _timer = cooldownTimer;
        _targetObj = null;
    }

    private void RetreatEnemy()
    {
        _timer = 0;
        _state = State.Retreat;
        _target = _initialPosition;
    }
    private void IdleEnemy()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _state = State.Idle;
    }

    #endregion
    
}
