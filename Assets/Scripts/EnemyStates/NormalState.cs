using UnityEngine;

public class NormalState : StateMachineBehaviour
{
    float timer;
    bool chasing;
    Enemy enemy;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemy)
            enemy = animator.GetComponent<Enemy>();

        timer = 0;
        chasing = false;
        enemy.SetBehaviour(EnemyBehaviour.Null);
        enemy.isFrightened = false;
        enemy.isDead = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            ChangeBehaviour();
    }

    private void ChangeBehaviour()
    {
        chasing = !chasing;
        if (chasing)
        {
            timer = enemy.chasingTime;
            enemy.SetBehaviour(EnemyBehaviour.ChasePlayer);
        }
        else
        {
            enemy.SetBehaviour(EnemyBehaviour.Null);
            timer = enemy.patrollingTime;
        }
    }    
}
