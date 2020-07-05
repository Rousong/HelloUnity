using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Cucumber : Enemy,IDamageble
{
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void SetOff()//animator event
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
