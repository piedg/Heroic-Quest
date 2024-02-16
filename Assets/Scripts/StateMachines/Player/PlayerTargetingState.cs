using HeroicQuest.Gameplay.Stats;
using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerTargetingState : PlayerMovementState
    {
        private readonly int targetingBlendTreeHash = Animator.StringToHash("TargetingLocomotion");
        private readonly int targetingForwardHash = Animator.StringToHash("TargetingForward");
        private readonly int targetingRightHash = Animator.StringToHash("TargetingRight");

        private const float animatorDampTime = 0.25f;
        private const float crossFadeDuration = 0.25f;

        Vector3 movement;
        float forwardAmount;
        float rightAmount;

        /* float nextStep;
         float stepRate = 0.7f;*/

        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Targeter.ShowIndicator();

            //stateMachine.InputManager.BlockEvent += OnBlock;

            stateMachine.InputManager.TargetEvent += OnTarget;
            stateMachine.InputManager.NextTargetEvent += NextTarget;
            stateMachine.InputManager.PrevTargetEvent += PrevTarget;
            stateMachine.InputManager.RollEvent += OnRoll;

            stateMachine.Health.OnTakeDamage += OnTakeDamage;

            stateMachine.Animator.CrossFadeInFixedTime(targetingBlendTreeHash, crossFadeDuration);
        }

        public override void Update(float deltaTime)
        {
            movement = CalculateMovement();


            if (stateMachine.Targeter.CurrentTarget.TryGetComponent(out Health enemy))
            {
                if (enemy.IsDead)
                {
                    NextTarget();
                    return;
                }
            }
            if (stateMachine.InputManager.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerMeleeAttackState(stateMachine, 0, movement));
                return;
            }

            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
                return;
            }

            movement.Normalize();
            ConvertDirection(movement);

            Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

            UpdateAnimator(deltaTime);
            FaceOnTarget(deltaTime);
        }

        public override void Exit()
        {
            // stateMachine.InputManager.BlockEvent -= OnBlock;
            stateMachine.InputManager.TargetEvent -= OnTarget;
            stateMachine.InputManager.NextTargetEvent -= NextTarget;
            stateMachine.InputManager.PrevTargetEvent -= PrevTarget;
            stateMachine.InputManager.RollEvent -= OnRoll;
            stateMachine.Health.OnTakeDamage -= OnTakeDamage;
        }

        void ConvertDirection(Vector3 direction)
        {
            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }

            Vector3 localMove = stateMachine.transform.InverseTransformDirection(direction);

            rightAmount = localMove.x;
            forwardAmount = localMove.z;
        }

        private void UpdateAnimator(float deltaTime)
        {
            stateMachine.Animator.SetFloat(targetingForwardHash, forwardAmount, animatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(targetingRightHash, rightAmount, animatorDampTime, deltaTime);
        }

        private void OnTarget()
        {
            stateMachine.Targeter.Cancel();
            stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
        }

        void OnRoll()
        {
            stateMachine.SwitchState(new PlayerRollState(stateMachine, movement));
        }

        void NextTarget()
        {
            stateMachine.Targeter.NextTarget();
        }

        void PrevTarget()
        {
            stateMachine.Targeter.PrevTarget();
        }
    }
}
