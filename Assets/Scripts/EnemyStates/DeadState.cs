using UnityEngine;

public class DeadState : StateMachineBehaviour
{
    [SerializeField] float respawnTime = 10;
    [SerializeField] string nextStateTrigger;
    float timer;

    Enemy enemy;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = respawnTime;

        if (!enemy)
            enemy = animator.GetComponent<Enemy>();

        //enemy.GetComponent<CharacterMovement>().Stop();
        enemy.ToggleMovement(false);

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            animator.SetTrigger(nextStateTrigger);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.ToggleMovement(true);
    }
}
