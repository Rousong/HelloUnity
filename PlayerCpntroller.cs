using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class PlayerCpntroller : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;
    public Collider2D coll;

    [Range(100, 500)]
    public float speed = 300;
    public float jumpForce;
    public int cherry;

    public Text cherryNum;

    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;
    private bool isHurt = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isHurt)
        {
            PlayerMove();
        }
        SwitchAnim();
    }

    private void PlayerMove()
    {
        float horizaontalMove = Input.GetAxis("Horizontal");
        // raw 的话就是整数 1 0 -1
        float faceDirection = Input.GetAxisRaw("Horizontal");

        if (horizaontalMove != 0)
        {
            rb.velocity = new Vector2(horizaontalMove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", Math.Abs(faceDirection));
        }
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        // Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            anim.SetBool("jumping", true);

        }
    }

    void SwitchAnim()
    {
        anim.SetBool("idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }

        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running",0);
            if (Mathf.Abs(rb.velocity.x) < 5f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }

        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //アイテム収集
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNum.text = cherry.ToString();
        }
    }
    //敵を倒す
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                // Destroy(collision.gameObject);
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
            }
        }

    }
}

