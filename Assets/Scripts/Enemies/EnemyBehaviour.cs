using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamageble<int>
{
    delegate void StateDelegate();
    StateDelegate State;
    StateDelegate Patrol;
    StateDelegate Chase;

    [SerializeField] protected float _atackSpeed = 1;
    protected float _lateralWallDetectDist = 1;
    protected float _detectDist = 10;
    protected int _damage = 2;
    protected float _hp = 15;
    protected float _speed = 5;


    private Collider2D _player;
    private RaycastHit2D[] _hits;

    private Rigidbody2D _rb;
    private Animator _anim;
    private WaitForSeconds PatrolWait = new WaitForSeconds(2);
    private Coroutine _turnCoroutine;
    private Coroutine _attackCoroutine;
    private bool _waiting = false;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        SetPatrolState();
        SetChaseState();
        State = Patrol;
    }

    protected virtual void Update()
    {
        State();
        SetAnimatorVariables();
        CheckDeath();
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
        _turnCoroutine = null;
    }

    private IEnumerator AttackPlayer()
    {
        _anim.Play("Attacking");
        yield return new WaitForSeconds(_atackSpeed);
        _attackCoroutine = null;
    }

    private void MoveFoward()
    {
        if (!_waiting)
        {
            _rb.velocity = new Vector2(transform.right.x * _speed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = Vector2.up * _rb.velocity.y;
        }
    }

    private void StopMoving()
    {
        _rb.velocity = Vector2.up * _rb.velocity.y;
    }

    private void CheckGround()
    {
        Debug.DrawRay(transform.position, ((Vector2)transform.right) * _lateralWallDetectDist);
        if (!Physics2D.Raycast(transform.position, (Vector2)transform.right - Vector2.up, 5f, LayerMask.GetMask("GroundOrWall")))
        {
            if (_turnCoroutine == null)
            {
                _turnCoroutine = StartCoroutine(PatrolTurnAround());
            }
        }
        if (Physics2D.Raycast(transform.position, (Vector2)transform.right, _lateralWallDetectDist, LayerMask.GetMask("GroundOrWall")))
        {
            if (_turnCoroutine == null)
            {
                _turnCoroutine = StartCoroutine(PatrolTurnAround());
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

    private void SetChaseState()
    {
        Chase += LookToPlayerDirection;
        Chase += WalkToPlayer;
        Chase += DetectPlayer;
    }

    private void WalkToPlayer()
    {
        if ((Mathf.Abs(transform.position.x - _player.transform.position.x)) >= 3)
        {
            _rb.velocity = ((Vector2.right * (_player.transform.position.x - transform.position.x)).normalized * _speed) + Vector2.up * _rb.velocity.y;
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackPlayer());
            }
        }
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

    public void ReciveDamage(int damage)
    {
        _hp -= damage;
        _anim.Play("Hit");
    }

    private void DeadDamage()
    {
        Collider2D circleCollider = GetComponentInChildren<CircleCollider2D>();
        Collider2D[] overlaps = new Collider2D[3];
        ContactFilter2D filter = new ContactFilter2D();
        circleCollider.OverlapCollider(filter, overlaps);
        foreach (Collider2D colider in overlaps)
        {
            if (colider.GetComponent<IDamageble<int>>() != null && colider.tag == "Player")
             {
                colider.gameObject.GetComponent<IDamageble<int>>().ReciveDamage(_damage);
                PlayerClass player = colider.GetComponent<PlayerClass>();
                StartCoroutine(player.KnockBackCoroutine());
                player.GetComponent<Rigidbody2D>().velocity = (Vector2.right * (_player.transform.position.x - transform.position.x)) * 5 + Vector2.up * 5;
             }
        }
    }

    private void CheckDeath()
    {
        if (_hp <= 0)
        {
            State = CheckDeath;
            _rb.velocity = Vector2.up * _rb.velocity.y;
            _anim.Play("Dying");
        }
    }

    private void DestroyThisEnemy()
    {
        Destroy(this.gameObject);
    }
}
