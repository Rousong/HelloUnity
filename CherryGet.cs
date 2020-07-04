using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CherryGet : MonoBehaviour
{   
    public void Death()
    {
        FindObjectOfType<PlayerCpntroller>().CherryCount();
        Destroy(gameObject);
    }

}
