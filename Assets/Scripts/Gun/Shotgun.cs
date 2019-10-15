using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunClass
{
    protected override void Start()
    {
        base.Start();
        _bulletsToShoot = 5;
        _shootDelay = .5f;
        _magSize = 4;
        _bullets = 5 * _magSize;
    }
}
