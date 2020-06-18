using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
    }

}
