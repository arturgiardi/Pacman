using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    List<Vector2> avaliableDirection = new List<Vector2>();
    [SerializeField] float wallCheckerDistance;
    [SerializeField] LayerMask whatIsWall;

    string enemyTag = "Enemy";

    private void Start()
    {
        GetAvaliableDirections();
    }

    private void GetAvaliableDirections()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, wallCheckerDistance, whatIsWall);
        if (!hit)
            avaliableDirection.Add(Vector2.up);
        
        hit = Physics2D.Raycast(transform.position, Vector2.down, wallCheckerDistance, whatIsWall);
        if (!hit)
            avaliableDirection.Add(Vector2.down);
        
        hit = Physics2D.Raycast(transform.position, Vector2.right, wallCheckerDistance, whatIsWall);
        if (!hit)
            avaliableDirection.Add(Vector2.right);

        hit = Physics2D.Raycast(transform.position, Vector2.left, wallCheckerDistance, whatIsWall);
        if (!hit)
            avaliableDirection.Add(Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
            collision.GetComponent<Enemy>().ChangeDirection(avaliableDirection);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag) && collision.GetComponent<CharacterMovement>().direction == Vector2.zero)
            collision.GetComponent<Enemy>().ChangeDirection(avaliableDirection);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * wallCheckerDistance), Color.red);
    }
}
