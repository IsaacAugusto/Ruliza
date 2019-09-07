using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunClass : MonoBehaviour
{
    public float Angle;
    protected float _damage = 1;
    protected float _magSize = 12;
    protected float _reloadTime = 5;
    protected float _shootDelay = .2f;
    private ReloadBar _reloadInterface;
    private float _timeSinceLastShoot;
    private float _delay;
    [SerializeField] private Transform _bulletSpawnPosition;
    [SerializeField] private bool _canShoot;
    [SerializeField] private float _bulletsShooted;
    private SpriteRenderer _sprite;
    private Camera _camera;
    private SpriteRenderer _playerSprite;
    private Rigidbody2D _playerRb;
    private Animator _animator;
    private Animator _playerAnimator;
    private AudioSource _source;

    virtual protected void Start()
    {
         _reloadInterface = FindObjectOfType<ReloadBar>();
        _camera = FindObjectOfType<Camera>();
        _sprite = GetComponent<SpriteRenderer>();
        _playerSprite = FindObjectOfType<PlayerClass>().GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerRb = _playerSprite.GetComponent<Rigidbody2D>();
        _playerAnimator = _playerSprite.GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        _delay = _shootDelay;
        _canShoot = true;
    }

    virtual protected void Update()
    {
        AimAtMouse();
        Fire();
        DelayCount();
        ReloadSystem();
    }

    private void AimAtMouse()
    {
        var dir = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
        Angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _sprite.flipY = (Mathf.Abs(Angle) > 90);

        if ((Mathf.Abs(Angle) > 90))
        {
            _playerSprite.transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            _playerSprite.transform.rotation = Quaternion.Euler(0,0,0);
        }

        transform.rotation = Quaternion.AngleAxis(-Angle, Vector3.back);
    }

    private void DelayCount()
    {
        if (_delay >= 0)
        {
            _delay -= Time.deltaTime;
        }
    }



    private void ReloadSystem()
    {
        if (_bulletsShooted > 0)
        {
            if (Time.time - _timeSinceLastShoot >= _reloadTime / 6)
            {
                _bulletsShooted -= Time.deltaTime * 5;
            }
            if (_bulletsShooted >= _magSize && _canShoot)
            {
                _canShoot = false;
                StartCoroutine(ReloadCoroutine(_reloadTime));
            }
        }

        _reloadInterface.ReloadBarFill(_bulletsShooted, _magSize);
    }

    private IEnumerator ReloadCoroutine(float reloadTime)
    {
        _canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        _bulletsShooted = 0;
        _canShoot = true;
    }

    protected void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if (_delay < 0 && _canShoot)
            {
                _timeSinceLastShoot = Time.time;
                var bullet = BulletPool.Instance.GetBulletFromPool();
                bullet.GetComponent<BulletClass>().Damage = _damage;
                bullet.transform.position = _bulletSpawnPosition.position;
                _source.Play();
                bullet.transform.rotation = transform.rotation;
                _delay = _shootDelay;
                _bulletsShooted++;

                CameraAnimator.ShakeCamera();
                _animator.SetTrigger("Shoot");
            }
        }
    }
}
