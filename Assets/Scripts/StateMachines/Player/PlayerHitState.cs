using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerHitState : PlayerBaseState
    {
        private readonly int HitHash = Animator.StringToHash("Hit");
        private const float CrossFadeduration = 0.1f;

        public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            stateMachine.Animator.CrossFadeInFixedTime(HitHash, CrossFadeduration);
        }

        public override void Enter() { }

        public override void Update(float deltaTime)
        {
            //Move(deltaTime);

            if (GetNormalizedTime(stateMachine.Animator, "Hit") >= 1)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
        }
    }
}
