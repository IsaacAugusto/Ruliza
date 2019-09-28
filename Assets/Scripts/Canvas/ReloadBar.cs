using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Image _frontImage;
    [SerializeField] private Image _HealthImage;
    [SerializeField] private Text _bullets;
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
        _frontImage.fillAmount = 1 - (percentual * bulletsShooted);
    }

    public void ShowBullets(int bullets)
    {
        _bullets.text = bullets.ToString();
    }

    public void HealthBarFill(float health, float maxhealth)
    {
        _HealthImage.fillAmount = (1 / maxhealth) * health;
    }

    private void ChangeColor()
    {
        if (_frontImage.fillAmount > 0.7f)
        {
            _frontImage.color = Color.green;
        }
        else if (_frontImage.fillAmount > 0.4f && _frontImage.fillAmount < 0.7f)
        {
            _frontImage.color = Color.yellow;
        }
        else
        {
            _frontImage.color = Color.red;
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
