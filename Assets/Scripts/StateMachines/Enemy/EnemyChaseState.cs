using HeroicQuest.StateMachine.Enemy;
using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{

    public class EnemyChaseState : EnemyBaseState
    {
        private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedHash = Animator.StringToHash("Speed");

        private const float CrossFadeduration = 0.1f;
        private const float AnimatorDumpTime = 0.1f;

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeduration);

        }

        public override void Update(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                //   stateMachine.SwitchState(new EnemySuspicionState(stateMachine));
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
            stateMachine.Animator.SetFloat(SpeedHash, stateMachine.Agent.velocity.magnitude, AnimatorDumpTime, deltaTime);
        }

        public override void Exit()
        {
            ResetAgentPath();
        }
    }
}