using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] Transform warpPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = warpPoint.transform.position;
    }
}
