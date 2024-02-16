using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyDeadState : EnemyBaseState
    {
        private readonly int DeadHash = Animator.StringToHash("Dead");
        private const float CrossFadeduration = 0.1f;
        float disableEnemyDelay = 5f;

        public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("Nemico Morto");
            disableEnemyDelay += Time.time;
            stateMachine.Animator.CrossFadeInFixedTime(DeadHash, CrossFadeduration);
            stateMachine.Controller.enabled = false;
        }

        public override void Update(float deltaTime)
        {
        }

        public override void Exit()
        {
        }


    }
}
