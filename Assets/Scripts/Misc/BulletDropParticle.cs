using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDropParticle : MonoBehaviour
{
    [SerializeField] private int _type = 2;
    private Transform _playerTransform;
    private GunClass _gun;
    private ParticleSystem _particle;
    private float _timer = 2;

    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerClass>().transform;
        if (_type == 1)
        {
            _gun = _playerTransform.GetChild(0).GetComponent<GunClass>();
        }
        else if (_type == 2)
        {
            _gun = _playerTransform.GetChild(1).GetComponent<GunClass>();
        }
        _particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0)
        {
            _particle.gravityModifier = 0;
            transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position + Vector3.up, .5f);
        }

        if (Vector2.Distance(transform.position, _playerTransform.position) <= 1)
        {
            GiveBullets();
        }
    }

    private void GiveBullets()
    {
        if (_type == 1)
        {
            _gun.AddBullets(3);
        }
        else if (_type == 2)
        {
            _gun.AddBullets(6);
        }
        gameObject.SetActive(false);
    }

}
