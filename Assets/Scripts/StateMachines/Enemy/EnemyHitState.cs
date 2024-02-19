using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyHitState : EnemyBaseState
    {
        private readonly int HitHash = Animator.StringToHash("Hit");
        private const float CrossFadeduration = 0.1f;

        public EnemyHitState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            stateMachine.Animator.CrossFadeInFixedTime(HitHash, CrossFadeduration);
        }

        public override void Enter() { }

        public override void Update(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Hit");
            if (normalizedTime >= 1)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }

        public override void Exit() { }
    }
}
