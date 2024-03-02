using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedHash = Animator.StringToHash("Speed");

        private float crossFadeDuration = 0.1f;
        private const float animatorDumpTime = 0.1f;

        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, crossFadeDuration);

            stateMachine.Health.OnTakeDamage += OnTakeDamage;
        }

        public override void Update(float deltaTime)
        {
            Move(deltaTime);

            if (IsInChaseRange() && (IsInViewRange() || IsPlayerNear()))
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedHash, 0f, animatorDumpTime, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.Health.OnTakeDamage -= OnTakeDamage;
        }
    }
}
