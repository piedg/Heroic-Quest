using Unity.VisualScripting;
using UnityEngine;

namespace TheNecromancers.StateMachine.Player
{
    public class PlayerLocomotionState : PlayerMovementState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParamHash = Animator.StringToHash("Speed");

        private const float AnimatorDumpTime = 0.1f;
        private const float CrossFadeDuration = 0.25f;

        Vector3 movement;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.SetFloat(SpeedParamHash, 0f);
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);

            stateMachine.InputManager.RollEvent += OnRoll;
        }

        public override void Update(float deltaTime)
        {
            movement = CalculateMovement();

            if (stateMachine.InputManager.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerMeleeAttackState(stateMachine, 0, movement));
                return;
            }

            Move(movement * stateMachine.MovementSpeed, deltaTime);

            if (stateMachine.InputManager.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(SpeedParamHash, 0f, AnimatorDumpTime, deltaTime);
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParamHash, stateMachine.Controller.velocity.magnitude, AnimatorDumpTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputManager.RollEvent -= OnRoll;
        }

        void OnRoll()
        {
            stateMachine.SwitchState(new PlayerRollState(stateMachine, movement));
            return;
        }
    }
}