using Unity.VisualScripting;
using UnityEngine;

namespace TheNecromancers.StateMachine.Player
{
    public class PlayerLocomotionState : PlayerMovementState
    {
        private readonly int locomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int speedParamHash = Animator.StringToHash("Speed");

        private const float animatorDampTime = 0.1f;
        private const float crossFadeDuration = 0.25f;

        Vector3 movement;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.SetFloat(speedParamHash, 0f);
            stateMachine.Animator.CrossFadeInFixedTime(locomotionBlendTreeHash, crossFadeDuration);

            stateMachine.InputManager.RollEvent += OnRoll;
            stateMachine.InputManager.BlockEvent += OnBlock;
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
                stateMachine.Animator.SetFloat(speedParamHash, 0f, animatorDampTime, deltaTime);
                return;
            }

            stateMachine.Animator.SetFloat(speedParamHash, stateMachine.Controller.velocity.magnitude, animatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputManager.RollEvent -= OnRoll;
            stateMachine.InputManager.BlockEvent -= OnBlock;
        }

        void OnRoll()
        {
            stateMachine.SwitchState(new PlayerRollState(stateMachine, movement));
            return;
        }

        void OnBlock()
        {
            stateMachine.SwitchState(new PlayerBlockState(stateMachine));
            return;
        }
    }
}