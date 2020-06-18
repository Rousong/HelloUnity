using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;

    public Transform leftPoint, rightPoint;
    private bool faceLeft = true;
    public float speed,jumpForce;
    private float leftX, rightX;
     public LayerMask ground;
    protected override void Start()
    {
        //获得父级的start
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        ///anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;

        // 断绝子项目的关系 这样左右的两个物体就不会跟着父物体移动
        transform.DetachChildren();
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);

    }

    private void Update()
    {
        //用动画事件去调用movement

        switchAnim();
    }
    void Movement()
    {
        if (faceLeft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                //anim.SetBool("frog", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
           
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = true;
            }
        }
    }

    void switchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground)&&anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }
}
