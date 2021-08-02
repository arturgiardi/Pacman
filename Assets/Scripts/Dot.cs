using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    GameObject player;
    [SerializeField] bool isEnergyzer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(GameManager.instance.playerTag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (isEnergyzer)
                GameManager.instance.GetEnergyzer();
            else
                GameManager.instance.GetDot();
            Destroy(gameObject);
        }
    }
}
