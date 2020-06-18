using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Eagle : Enemy
{
    private Rigidbody2D rb;
    public Transform upPoint, downPoint;
    public float speed;
    private float upY, downY;

    private bool isUp;

    protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        // 断绝子项目的关系 这样左右的两个物体就不会跟着父物体移动
        
        upY = upPoint.position.y;
        downY = downPoint.position.y;

        // transform.DetachChildren();
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);

    }

    private void Update()
    {
        MoveMent();
    }


    void MoveMent()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y >  upY)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < downY)
            {
                isUp = true;
            }
        }
    }
}
