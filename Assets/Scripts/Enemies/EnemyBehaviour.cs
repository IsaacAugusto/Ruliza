using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    delegate void StateDelegate();
    StateDelegate State;
    StateDelegate Patrol;
    StateDelegate Chase;


    private Collider2D _player;
    private RaycastHit2D[] _hits;

    private Rigidbody2D _rb;
    private float _detectDist = 10;
    private Animator _anim;
    private WaitForSeconds PatrolWait = new WaitForSeconds(2);
    private Coroutine _coroutine;
    private bool _waiting = false;
    private float _speed = 5;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        SetPatrolState();
        SetChaseState();
        State = Patrol;
    }

    void Update()
    {
        State();
        SetAnimatorVariables();
    }

    private void SetPatrolState()
    {
        Patrol += CheckGround;
        Patrol += MoveFoward;
        Patrol += DetectPlayer;
    }

    private IEnumerator PatrolTurnAround()
    {
        _waiting = true;
        yield return PatrolWait;
        TurnAround();
        _waiting = false;
        _coroutine = null;
    }

    private void MoveFoward()
    {
        if (!_waiting)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.velocity = new Vector2(transform.right.x * _speed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void StopMoving()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void CheckGround()
    {
        if (!Physics2D.Raycast(transform.position, (Vector2)transform.right - Vector2.up, 5f, LayerMask.GetMask("GroundOrWall")))
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(PatrolTurnAround());
            }
        }
        if (Physics2D.Raycast(transform.position, (Vector2)transform.right, 1f, LayerMask.GetMask("GroundOrWall")) ||
            Physics2D.Raycast((Vector2)transform.position + (Vector2)transform.right, (Vector2)transform.right, .5f, LayerMask.GetMask("Enemy")))
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(PatrolTurnAround());
            }
        }
    }

    private void DetectPlayer()
    {
        _player = Physics2D.OverlapCircle(transform.position, _detectDist, LayerMask.GetMask("Player"));
        if (_player != null)
        {
            _hits = Physics2D.RaycastAll(transform.position, _player.transform.position - transform.position);
            if (_hits[1].transform == _player.transform)
            {
                State = Chase;
            }
            else
            {
                State = Patrol;
            }
        }
        else
        {
            State = Patrol;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,  _player.transform.position - transform.position);
    }

    private void SetChaseState()
    {
        Chase += LookToPlayerDirection;
        Chase += WalkToPlayer;
        Chase += DetectPlayer;
    }

    private void WalkToPlayer()
    {
        _rb.velocity = ((Vector2.right * (_player.transform.position.x - transform.position.x)).normalized * _speed);
    }

    private void LookToPlayerDirection()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (_player.transform.position.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void TurnAround()
    {
        if (transform.rotation.y == 0)
        {
            transform.Rotate(0, 180, 0);
        }
        else
        {
            transform.Rotate(0, -180, 0);
        }
    }

    #region Animator
    private void SetAnimatorVariables()
    {
        _anim.SetInteger("VelX", Mathf.Abs((int)_rb.velocity.x));
    }
    #endregion
}
