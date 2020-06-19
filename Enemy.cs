using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }
    public void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        deathAudio.Play();
        anim.SetTrigger("death");
    }

}
