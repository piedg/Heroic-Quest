using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerLocomotionState : PlayerMovementState
    {
        private readonly int locomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int speedParamHash = Animator.StringToHash("Speed");

        private const float animatorDampTime = 0.25f;
        private const float crossFadeDuration = 0.25f;

        Vector3 movement;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.SetFloat(speedParamHash, 0f);
            stateMachine.Animator.CrossFadeInFixedTime(locomotionBlendTreeHash, crossFadeDuration);

            stateMachine.InputManager.RollEvent += OnRoll;
            stateMachine.InputManager.BlockEvent += OnBlock;
            stateMachine.InputManager.TargetEvent += OnTarget;
            stateMachine.InputManager.InteractEvent += OnInteract;
            stateMachine.InputManager.EquipPrimary += OnEquipPrimary;

            stateMachine.Health.OnTakeDamage += OnTakeDamage;
        }

        public override void Update(float deltaTime)
        {
            movement = CalculateMovement();

            OnAttack();

            Move(movement * stateMachine.MovementSpeed, deltaTime);

            if (stateMachine.InputManager.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(speedParamHash, 0f, animatorDampTime, deltaTime);
            }

            stateMachine.Animator.SetFloat(speedParamHash, stateMachine.Controller.velocity.magnitude, animatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputManager.RollEvent -= OnRoll;
            stateMachine.InputManager.BlockEvent -= OnBlock;
            stateMachine.InputManager.TargetEvent -= OnTarget;
            stateMachine.InputManager.InteractEvent -= OnInteract;
            stateMachine.InputManager.EquipPrimary -= OnEquipPrimary;

            stateMachine.Health.OnTakeDamage -= OnTakeDamage;
        }

        protected void OnRoll()
        {
            stateMachine.SwitchState(new PlayerRollState(stateMachine, movement));
        }

        void OnAttack()
        {
            if (stateMachine.InputManager.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerMeleeAttackState(stateMachine, 0, movement));
            }
        }

        void OnTarget()
        {
            if (!stateMachine.Targeter.SelectTarget()) { return; }
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }
}