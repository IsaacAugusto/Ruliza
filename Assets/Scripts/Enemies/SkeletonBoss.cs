using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : EnemyBehaviour
{
    [SerializeField] private GameObject _enemyBullet;
    [SerializeField] private GameObject _arena;
    [SerializeField] private BoxCollider2D _arenaEnter;
    private Transform _player;
    private Camera _camera;

    protected override void Start()
    {
        base.Start();
        _enemyBullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
        _enemyBullet.SetActive(false);
        _hp = 35;
        _lateralWallDetectDist = 4;
        _speed = 5;
        _atackDistance = 5;
        _player = FindObjectOfType<PlayerClass>().transform;
        _camera = FindObjectOfType<Camera>();
    }

    protected override void Update()
    {
        base.Update();
        if (_arenaEnter.IsTouchingLayers(LayerMask.GetMask("Player")) && !_arena.activeInHierarchy)
        {
            _arena.SetActive(true);
        }
    }

    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (_hp <= 0)
        {
            _arena.SetActive(false);
        }
    }

    public override void ReciveDamage(int damage)
    {
        base.ReciveDamage(damage);

        int x = Random.Range(0, 10);


        if (!_enemyBullet.activeInHierarchy && x >= 8)
        {
            var dir = _camera.WorldToScreenPoint(_player.position) - _camera.WorldToScreenPoint(transform.position);
            var Angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _enemyBullet.transform.rotation = Quaternion.AngleAxis(-Angle, Vector3.back);

            _enemyBullet.transform.position = transform.position + transform.right;
            _enemyBullet.SetActive(true);
        }
    }
}
