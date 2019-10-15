using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
    private static Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();    
    }

    public static void ShakeCamera()
    {
        _anim.Play("ShakeCamera");
    }
}
