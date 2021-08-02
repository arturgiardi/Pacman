using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedState : StateMachineBehaviour
{
    [SerializeField] float timeFrightened = 10;
    [SerializeField] string nextStateTrigger;
    float timer;

    Enemy enemy;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = timeFrightened;

        if (!enemy)
            enemy = animator.GetComponent<Enemy>();

        enemy.SetBehaviour(EnemyBehaviour.MoveAwayFromPlayer);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            animator.SetTrigger(nextStateTrigger);
    }
}
