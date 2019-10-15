using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerClass : MonoBehaviour, IDamageble<int>
{
    #region Variables
    private bool IsStuned;

    private Rigidbody2D _rb;
    private AudioSource _source;
    private Animator _anim;
    private GunClass _gun;

    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _jumpForce;
    [SerializeField] private LayerMask _groundOrWallLayer;

    private ReloadBar _reloadInterface;

    protected float _maxHp = 3;
    protected float _hp;
    private float _xInput;
    private float _fallMult = 2.5f;
    private float _lowFallMult = 2f;
    private bool _isGrounded;
    #endregion

    protected abstract void SetStatus(float MaxHP);

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _gun = GetComponentInChildren<GunClass>();
        _reloadInterface = FindObjectOfType<ReloadBar>();
    }

    protected virtual void Update()
    {
        MoveAndJump();
        SetAnimVariables();
        ChekPlayerHp();
        _reloadInterface.HealthBarFill(_hp, _maxHp);
    }

    public void ReciveDamage(int damage) // Recive damage passed as argument
    {
        _anim.Play("Ruhan_Hit");
        _hp -= damage;
    }

    private void GetXInputs()// Get A and D inputs
    {
        _xInput = Input.GetAxisRaw("Horizontal");
    }

    private void CheckGround()// Check if player is toching the ground
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, (transform.localScale.y + 1.5f), _groundOrWallLayer)
        || Physics2D.Raycast(transform.position + (Vector3.right) * transform.localScale.x/2, Vector2.down, (transform.localScale.y + 1.5f), _groundOrWallLayer)
        || Physics2D.Raycast(transform.position + (Vector3.left) * transform.localScale.x/2, Vector2.down, (transform.localScale.y + 1.5f), _groundOrWallLayer);
    }

    private void MoveAndJump()// Controlls move and jump
    {
        GetXInputs();
        CheckGround();

        if (!IsStuned)
        {
            _rb.velocity = new Vector2(_xInput * _moveSpeed, _rb.velocity.y); // Movement

            if (Input.GetKeyDown(KeyCode.W)) // Jump
            {
                if (_isGrounded)
                {
                    _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
            }
        }


        if (_rb.velocity.y < 0) // Better Jump, better fall
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMult - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space)))
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowFallMult - 1) * Time.deltaTime;
        }

        _anim.SetFloat("AbsVelX", Mathf.Abs(_rb.velocity.x));
        _anim.SetFloat("VelY", _rb.velocity.y);
    }

    public void SetStuned(bool value)
    {
        IsStuned = value;
    }

    public IEnumerator KnockBackCoroutine()
    {
        SetStuned(true);
        yield return new WaitForSeconds(.5f);
        SetStuned(false);
    }

    private void ChekPlayerHp()
    {
        if (_hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetAnimVariables()
    {
        // Set Run direction by aim position
        if (_gun.Angle > 90 || _gun.Angle < -90)
        {
            if (_rb.velocity.x >= .5f)
            {
                _anim.SetFloat("LookBack", 1);
            }
            else if (_rb.velocity.x <= -.5f)
            {
                _anim.SetFloat("LookBack", 0);
            }
        }
        else
        {
            if (_rb.velocity.x >= .5f)
            {
                _anim.SetFloat("LookBack", 0);
            }
            else if (_rb.velocity.x <= -.5f)
            {
                _anim.SetFloat("LookBack", 1);
            }
        }
    }
}
