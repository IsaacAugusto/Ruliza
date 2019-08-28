using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Coroutine _coroutine;
    delegate void MoveDelegate();
    MoveDelegate Move;
    private bool _waiting = false;
    private float _speed = 5;
    void Start()
    {
        Move = MoveFoward;
        _rb = GetComponent<Rigidbody2D>();
        _speed = 5;
    }

    void Update()
    {
        Move();
        CheckGround();
        Debug.DrawRay(transform.position,((Vector2)transform.right -Vector2.up) * 5);
    }

    private IEnumerator PatrolTurnAround()
    {
        _waiting = true;
        Move = StopMoving;
        yield return new WaitForSeconds(2);
        TurnAround();
        _waiting = false;
        Move = MoveFoward;
        _coroutine = null;
    }

    private void MoveFoward()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.velocity = new Vector2(transform.right.x * _speed, _rb.velocity.y);
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
}
