using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuhanPlayer : PlayerClass
{
    [SerializeField] private int MaxHP = 5;

    protected override void SetStatus(int MaxHP)
    {
        _maxHp = MaxHP;
    }

    protected override void Start()
    {
        base.Start();
        SetStatus(MaxHP);
    }
}
