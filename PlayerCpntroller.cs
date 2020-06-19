using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Collider2D dissColl;
    public Transform cellingCheck;
    public AudioSource jumpAudio, hurtAudio,cherryAudio;

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
            rb.velocity = new Vector2(horizaontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Math.Abs(faceDirection));
        }
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        // Jump
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            anim.SetBool("jumping", true);

        }
        Crouch();
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
            cherryAudio.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNum.text = cherry.ToString();
        }
        // 掉落后死亡重置游戏
        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart",2);
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
            }// shou shang 
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                hurtAudio.Play();
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

    private void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground)) {

            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                dissColl.enabled = false;
            }
            else 
            {
                anim.SetBool("crouching", false);
                dissColl.enabled = true;
            }
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

