using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunClass : MonoBehaviour
{
    public Transform _bulletSpawnPosition;
    public float Angle;
    public bool _canShoot;
    protected int _damage = 1;
    protected int _bulletsToShoot = 1;
    protected int _bullets = 48;
    protected int _magSize = 12;
    protected float _reloadTime = 1;
    protected float _shootDelay = .2f;
    private ReloadBar _reloadInterface;
    private Coroutine _reloadCoroutine;
    private float _timeSinceLastShoot;
    private float _delay;
    [SerializeField] private int _bulletsShooted;
    private SpriteRenderer _sprite;
    private Camera _camera;
    private SpriteRenderer _playerSprite;
    protected Rigidbody2D _playerRb;
    private Animator _animator;
    private Animator _playerAnimator;
    private AudioSource _source;

    virtual protected void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _reloadInterface = FindObjectOfType<ReloadBar>();
        _sprite = GetComponent<SpriteRenderer>();
        _playerSprite = FindObjectOfType<PlayerClass>().GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerRb = _playerSprite.GetComponent<Rigidbody2D>();
        _playerAnimator = _playerSprite.GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        _delay = _shootDelay;
        _bulletSpawnPosition = transform.GetChild(0).transform;
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

    public void AddBullets(int quant)
    {
        _bullets += quant;
    }

    private void ReloadSystem()
    {
        if (_bulletsShooted >= _magSize)
        {
            _canShoot = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && _bullets > 0 && _reloadCoroutine == null)
        {
            _reloadCoroutine = StartCoroutine(ReloadCoroutine(_reloadTime));
        }

        _reloadInterface.ShowBullets(_bullets);
        _reloadInterface.ReloadBarFill(_bulletsShooted, _magSize);
    }

    private IEnumerator ReloadCoroutine(float reloadTime)
    {
        _canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        if (_bullets < _magSize)
        {
            if (_bullets <= _bulletsShooted)
            {
                _bulletsShooted -= _bullets;
                _bullets = 0;
            }
            else
            {
                _bullets -= _bulletsShooted;
                _bulletsShooted = 0;
            }
        }
        else
        {
            _bullets -= _bulletsShooted;
            _bulletsShooted = 0;
        }
        _canShoot = true;
        _reloadCoroutine = null;
    }

    protected virtual void BulletSpawn(int bulletsToShoot)
    {
        for (int i = 0; i < bulletsToShoot; i++)
        {
            var bullet = BulletPool.Instance.GetBulletFromPool();
            bullet.GetComponent<BulletClass>().Damage = _damage;
            bullet.transform.position = _bulletSpawnPosition.position;
            if (bulletsToShoot == 1)
            {
                bullet.transform.rotation = transform.rotation;
            }
            else
            {
                bullet.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-20, 20));
            }
            TrailRenderer trail = bullet.GetComponent<TrailRenderer>();
            trail.Clear();
        }
    }

    protected void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if (_delay < 0 && _canShoot)
            {
                _timeSinceLastShoot = Time.time;
                BulletSpawn(_bulletsToShoot);
                _source.Play();
                _delay = _shootDelay;
                _bulletsShooted++;

                CameraAnimator.ShakeCamera();
                _animator.SetTrigger("Shoot");
            }
        }
    }
}
