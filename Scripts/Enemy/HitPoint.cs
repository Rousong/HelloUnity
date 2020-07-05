using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class HitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.LogError("get hurt");
            collision.GetComponent<IDamageble>().GetHit(1);
        }
        if (collision.CompareTag("Bomb"))
        {

        }
    }
}
