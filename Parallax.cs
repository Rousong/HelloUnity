using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float startPointX,startPointY;
    public bool lockY;


    private void Start()
    {
        startPointX = transform.position.x;
    }

    private void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, transform.position.y);

        }
        else
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY + cam.position.y * moveRate);

        }
    }
}
