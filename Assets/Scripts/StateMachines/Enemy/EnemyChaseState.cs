using HeroicQuest.StateMachine.Enemy;
using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int speedHash = Animator.StringToHash("Speed");

        private const float crossFadeduration = 0.1f;
        private const float animatorDumpTime = 0.1f;

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(locomotionHash, crossFadeduration);

            stateMachine.Health.OnTakeDamage += OnTakeDamage;
        }

        public override void Update(float deltaTime)
        {
            if (!IsInChaseRange() && !IsInViewRange())
            {
                stateMachine.SwitchState(new EnemySuspicionState(stateMachine));
                return;
            }
            else if (IsInAttackRange())
            {
                if (stateMachine.IsRanged)
                {
                    //     stateMachine.SwitchState(new EnemyRangedAttackState(stateMachine));
                }
                else
                {
                    stateMachine.SwitchState(new EnemyAttackState(stateMachine));
                }
                return;
            }

            MoveToPlayer(deltaTime);
            FaceForward(deltaTime);
            stateMachine.Animator.SetFloat(speedHash, stateMachine.Agent.velocity.magnitude, animatorDumpTime, deltaTime);
        }

        public override void Exit()
        {
            ResetAgentPath();
            stateMachine.Health.OnTakeDamage -= OnTakeDamage;
        }
    }
}