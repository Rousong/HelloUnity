using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;
    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    public float attackRange, skillRange;
    private float nextAttack = 0;

    public List<Transform> attackList = new List<Transform>();


    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        TransitionToState(patrolState);
    }

    private void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            return;
        }
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }
    public void TransitionToState(EnemyBaseState state)
    {

        currentState = state;
        currentState.EnterState(this);
    }
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position,targetPoint.position,speed*Time.deltaTime);
        FilpDirection();
    }

    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f,0f,0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public virtual void AttackAction()//攻击玩家
    {
        
        if (Vector2.Distance(transform.position,targetPoint.position)<attackRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("attack");
                Debug.LogError("攻击玩家");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction()//对炸弹使用技能
    {
        
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("skill");
                Debug.LogError("对炸弹使用技能");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void SwtichPoint()
    {
        if (Mathf.Abs(pointA.position.x -transform.position.x)>Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform))
        {
            attackList.Add(collision.transform);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(OnAlarm());
    }
    // xie cheng 
    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);

    }
}
