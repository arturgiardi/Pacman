using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviour { Null, ChasePlayer, MoveAwayFromPlayer }
public class Enemy : MonoBehaviour
{
    [SerializeField] float _patrollingTime;
    [SerializeField] float _chasingTime;
    [SerializeField] Transform respawnPositionRef;
    [SerializeField] AudioClip deathSound;
    [SerializeField] CharacterMovement charMovement;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D col;

    Vector3 respawnPosition;
    Vector3 startPosition;
    Transform player;
    EnemyBehaviour enemyBehaviour;
    //List<Vector2> desiredDirections = new List<Vector2>();
    bool startPositionSetted;
    private string frightenString = "Frightened";
    private string deadString = "Dead";
    private bool canMove;
    private string normalTrigger = "Normal";

    public float patrollingTime => _patrollingTime;
    public float chasingTime => _chasingTime;

    public bool isDead { get; set; }
    public bool isFrightened { get; set; }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag(GameManager.instance.playerTag).transform;
        startPosition = transform.position;
        respawnPosition = (respawnPositionRef != null) ? respawnPositionRef.position : transform.position;
        startPositionSetted = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player)
        {
            if (isFrightened)
                Death();
            else
                GameManager.instance.LoseLife();
        }
    }

    private void ClearDesiredDirections(ref List<Vector2> desiredDirections)
    {
        desiredDirections.Clear();
    }

    internal void FindClosestDirectionsToPlayer(ref List<Vector2> desiredDirections)
    {
        desiredDirections.Clear();
        desiredDirections.Add(GetClosestDirectionToPlayer(Vector2.zero));
        desiredDirections.Add(GetClosestDirectionToPlayer(desiredDirections[0]));
    }

    internal void FindFurtherDirectionsToPlayer(ref List<Vector2> desiredDirections)
    {
        desiredDirections.Clear();
        desiredDirections.Add(GetFurtherDirectionToPlayer(Vector2.zero));
        desiredDirections.Add(GetFurtherDirectionToPlayer(desiredDirections[0]));
    }



    private Vector2 GetClosestDirectionToPlayer(Vector2 directionToExclude)
    {
        Vector2 direction = Vector2.up;
        float minDistance = Vector3.Distance(transform.position + Vector3.up, player.position);

        float distance = Vector3.Distance(transform.position + Vector3.left, player.position);
        if (minDistance > distance || directionToExclude == direction)
        {
            minDistance = distance;
            direction = Vector2.left;
        }
        distance = Vector3.Distance(transform.position + Vector3.right, player.position);
        if (minDistance > distance)
        {
            minDistance = distance;
            direction = Vector2.right;
        }
        distance = Vector3.Distance(transform.position + Vector3.down, player.position);
        if (minDistance > distance)
        {
            minDistance = distance;
            direction = Vector2.down;
        }

        return direction;
    }

    private Vector2 GetFurtherDirectionToPlayer(Vector2 directionToExclude)
    {
        Vector2 direction = Vector2.up;
        float maxDistance = Vector3.Distance(transform.position + Vector3.up, player.position);

        float distance = Vector3.Distance(transform.position + Vector3.left, player.position);
        if (maxDistance < distance || directionToExclude == direction)
        {
            maxDistance = distance;
            direction = Vector2.left;
        }
        distance = Vector3.Distance(transform.position + Vector3.right, player.position);
        if (maxDistance < distance)
        {
            maxDistance = distance;
            direction = Vector2.right;
        }
        distance = Vector3.Distance(transform.position + Vector3.down, player.position);
        if (maxDistance < distance)
        {
            maxDistance = distance;
            direction = Vector2.down;
        }

        return direction;
    }

    internal void Frighten()
    {
        if (!isDead)
        {
            animator.SetTrigger(frightenString);
            isFrightened = true;
        }
    }

    private void Death()
    {
        isDead = true;
        GameManager.instance.KillEnemy();
        SoundManager.instance.PlaySFX(deathSound);
        animator.SetTrigger(deadString);
        charMovement.Stop();
        transform.position = respawnPosition;
        isFrightened = false;
    }

    internal void ChangeDirection(List<Vector2> avaliableDirection)
    {
        if (isDead || !canMove)
        {
            charMovement.Stop();
            return;
        }

        List<Vector2> desiredDirections = new List<Vector2>();

        switch (enemyBehaviour)
        {
            case EnemyBehaviour.ChasePlayer:
                FindClosestDirectionsToPlayer(ref desiredDirections);
                break;
            case EnemyBehaviour.MoveAwayFromPlayer:
                FindFurtherDirectionsToPlayer(ref desiredDirections);
                break;
            default:
                break;
        }

        foreach (var item in desiredDirections)
        {
            if (avaliableDirection.Contains(item))
            {
                charMovement.SetNextDirection(item);
                return;
            }
        }

        charMovement.SetNextDirection(avaliableDirection[Random.Range(0, avaliableDirection.Count)]);
    }

    public void Initialize()
    {
        isDead = false;
        isFrightened = false;
        animator.SetTrigger(normalTrigger);
        if(startPositionSetted)
            transform.position = startPosition;
        ToggleMovement(false);
    }
    internal void StartLevel()
    {
        ToggleMovement(true);
    }

    public void ToggleMovement(bool value)
    {
        canMove = value;
        col.enabled = value;
        charMovement.Stop();
    }

    public void SetBehaviour(EnemyBehaviour behaviour)
    {
        enemyBehaviour = behaviour;
    }
}
