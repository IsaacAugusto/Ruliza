using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : EnemyBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _hp = 25;
        _lateralWallDetectDist = 4;
        _speed = 3;
    }
}
