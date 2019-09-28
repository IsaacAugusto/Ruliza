using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuhanPlayer : PlayerClass
{
    [SerializeField] private float MaxHP = 3;

    protected override void SetStatus(float MaxHP)
    {
        _maxHp = MaxHP;
        _hp = MaxHP;
    }

    protected override void Start()
    {
        base.Start();
        SetStatus(MaxHP);
    }
}
