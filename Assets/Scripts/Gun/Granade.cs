using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
       
    }


    private void DisableGranade()
    {
        this.gameObject.SetActive(false);
    }

    private void Explode()
    {
        Collider2D[] c;
        c = Physics2D.OverlapCircleAll(transform.position, 5);
        foreach (Collider2D co in c)
        {
            if (co.gameObject.GetComponent<IDamageble<int>>() != null)
            {
                co.gameObject.GetComponent<IDamageble<int>>().ReciveDamage(10);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _anim.Play("GranedeExplosion");
    }
}
