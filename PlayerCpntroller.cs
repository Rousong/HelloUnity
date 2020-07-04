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
    public Transform cellingCheck,groundCheck;
    //public AudioSource jumpAudio, hurtAudio,cherryAudio;

    [Range(1, 500)]
    public float speed = 10;
    public float jumpForce;
    public int cherry;
    private float horizontalMove;
    public Text cherryNum;

    public LayerMask ground;

    public bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;
    private bool isHurt = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        //NewJump();
        Crouch();
        cherryNum.text = cherry.ToString();
    }
    private void FixedUpdate()
    {

        // 检测玩家是否在地面上  在脚下这个点画一个圆圈  然后check 返回布尔型  0.1是检测范围 和ground这个layermask做检测
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
        if (!isHurt)
        {
            PlayerMove();
        }
        Jump();
        SwitchAnim();
    }

    private void PlayerMove()
    {
        // raw 的话就是整数 1 0 -1
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }

    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

    }

    //void NewJump()
    //{
    //    if (isGround)
    //    {
    //        jumpCount = 1;
    //        isJump = false;
    //    }
    //    if (Input.GetButtonDown("Jump") && jumpCount > 0)
    //    {
    //        rb.velocity = Vector2.up * jumpForce;
    //        jumpCount--;
    //        SoundManager.instance.JumpAudio();
    //        anim.SetBool("jumping", true);
    //    }
    //    if (Input.GetButtonDown("Jump") && jumpCount == 0 && isGround)
    //    {
    //        rb.velocity = Vector2.up * jumpForce;
    //        SoundManager.instance.JumpAudio();
    //        anim.SetBool("jumping", true);
    //    }
    //}

    void SwitchAnim()
    {
        //anim.SetBool("idle", false);
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
            print(anim.GetBool("falling"));
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
            print(rb.velocity.y);
        }
        //if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        //{
        //    anim.SetBool("falling", true);
        //}

        //if (anim.GetBool("jumping"))
        //{
        //    if (rb.velocity.y < 0)
        //    {
        //        anim.SetBool("jumping", false);
        //        anim.SetBool("falling", true);
        //    }
        //}
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running",0);
            if (Mathf.Abs(rb.velocity.x) < 5f)
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }

        }
        //else if (coll.IsTouchingLayers(ground))
        //{
        //    anim.SetBool("falling", false);
        //    //anim.SetBool("idle", true);
        //}
    }

    //アイテム収集
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            //cherryAudio.Play();
            //Destroy(collision.gameObject);
            //cherry += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //cherryNum.text = cherry.ToString();
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
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                SoundManager.instance.HurtAudio();
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

    public void CherryCount() {
        cherry++;
    }
}

