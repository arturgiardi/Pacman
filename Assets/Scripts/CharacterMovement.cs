using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float tileSize = 1.75f;
    [SerializeField] float speed = 2;
    [SerializeField] float wallCheckerDistance;
    [SerializeField] LayerMask whatIsWall;

    SpriteRenderer spriteRenderer;
    Vector2 nextDirection;

    public Vector2 direction { get; private set; }

    private void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        ChangeDirection();
        Move();
    }

    private void ChangeDirection()
    {
        if (nextDirection == direction)
            return;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * tileSize, 0, nextDirection,wallCheckerDistance, whatIsWall);

        if (!hit)
        {
            direction = nextDirection;
            if (direction == Vector2.right)
                spriteRenderer.flipX = false;
            else if(direction == Vector2.left)
                spriteRenderer.flipX = true;
        }
    }

    internal void Stop()
    {
        direction = Vector2.zero;
        nextDirection = Vector2.zero;
    }

    private void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckerDistance, whatIsWall);

        if (!hit)
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, speed*Time.fixedDeltaTime);
        else
            direction = Vector2.zero;
    }

    public void SetVerticalDirection(float value)
    {
        if (value > 0)
            nextDirection = Vector2.up;
        else if (value < 0)
            nextDirection = Vector2.down;
    }

    public void SetHorizontalDirection(float value)
    {
        if (value > 0)
            nextDirection = Vector2.right;
        else if (value < 0)
            nextDirection = Vector2.left;
    }

    public void SetNextDirection(Vector2 nextDirection)
    {
        this.nextDirection = nextDirection;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.left * wallCheckerDistance), Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * wallCheckerDistance), Color.red);
    }
}
