using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform _player;
    private Vector3 _speed = Vector3.zero;
    private float _smooth = 0.2f;
    private Vector3 _mousePos;
    private Camera _camera;
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _player = FindObjectOfType<PlayerClass>().transform;
    }

    void Update()
    {
        FollowPlayer();
        _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FollowPlayer()
    {
        if (_player)
        {
            Vector3 relativePos = Vector3.Lerp(_player.position, _mousePos, .1f);
            Vector3 pos = Vector3.SmoothDamp(transform.position, relativePos, ref _speed, _smooth);
            pos.z = -10;
            transform.position = pos;
        }
    }
}
