using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerRollState : PlayerMovementState
    {
        private readonly int dashBlendTreeHash = Animator.StringToHash("Roll");

        private const float crossFadeDuration = 0.3f;

        Vector3 direction;
        private float remainingRollTime;

        public PlayerRollState(PlayerStateMachine stateMachine, Vector3 direction) : base(stateMachine)
        {
            this.direction = direction;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(dashBlendTreeHash, crossFadeDuration);
            remainingRollTime = stateMachine.RollDuration;

            //AudioManager.Instance.PlayRandomClip(stateMachine.AudioClips.Roll);
        }

        public override void Update(float deltaTime)
        {
            remainingRollTime -= deltaTime;

            if (remainingRollTime <= 0f)
            {
                ReturnToLocomotion();
                return;
            }

            if (direction == Vector3.zero)
            {
                Move(stateMachine.transform.forward * stateMachine.RollForce, deltaTime);
                return;
            }

            Move(direction.normalized * stateMachine.RollForce, deltaTime);
            FaceMovementDirection(direction, deltaTime);
        }

        public override void Exit() { }
    }
}