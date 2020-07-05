using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    public float startTime;
    public float waitTime;
    public float bombForce;
    

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;

    }

    private void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("explotion");
            }
        }
      
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }

    public void Explotion()// animation event
    {
        coll.enabled = false;
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position,radius,targetLayer);
        rb.gravityScale = 0;

        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;

            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up) * bombForce, ForceMode2D.Impulse);
            if (item.CompareTag("Bomb")&& item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            if (item.CompareTag("Player"))
            {
                item.GetComponent<IDamageble>().GetHit(3);
            }
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
    public void TurnOff()
    {
        anim.Play("bomb_off");
        //改成和敌人一样的土城 这样炸弹就不会爆炸了
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }
    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }

}
