﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Image _frontImage;
    [SerializeField] private Transform _player;
    public static Camera MyCamera;

    void Start()
    {
        MyCamera = FindObjectOfType<Camera>();
        _player = FindObjectOfType<PlayerClass>().transform;
    }

    void Update()
    {
        FollowPlayer();
        ChangeColor();
    }

    public void ReloadBarFill(float bulletsShooted, float magSize)
    {
        var percentual = 1 / magSize;
        _frontImage.fillAmount = percentual * bulletsShooted;
    }

    private void ChangeColor()
    {
        if (_frontImage.fillAmount > 0.7f)
        {
            _frontImage.color = Color.red;
        }
        else if (_frontImage.fillAmount > 0.4f && _frontImage.fillAmount < 0.7f)
        {
            _frontImage.color = Color.yellow;
        }
        else
        {
            _frontImage.color = Color.green;
        }
    }

    private void FollowPlayer()
    {
        if (_player)
        {
            transform.position = MyCamera.WorldToScreenPoint(_player.position + Vector3.up * 2.2f);
        }
    }
}
