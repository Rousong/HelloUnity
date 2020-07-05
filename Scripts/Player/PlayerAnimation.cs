using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        anim.SetFloat("speed", math.abs(rb.velocity.x));
        anim.SetFloat("velocityY",rb.velocity.y);
        anim.SetBool("jump",controller.isJump);
        anim.SetBool("ground", controller.isGround);
    }
}
