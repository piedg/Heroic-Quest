using HeroicQuest.Gameplay.Combat.Attack;
using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerMeleeAttackState : PlayerMovementState
    {
        private float previousFrameTime;
        private bool alreadyAppliedForce;

        private Attack attack;
        private int attackIndex;
        private Vector3 direction;

        public PlayerMeleeAttackState(PlayerStateMachine stateMachine, int attackIndex, Vector3 direction) : base(stateMachine)
        {
            this.attackIndex = attackIndex;
            this.attack = stateMachine.CurrentWeapon.Attacks[attackIndex];
            this.direction = direction;
        }

        public override void Enter()
        {
            stateMachine.InputManager.TargetEvent += OnTarget;

            direction = CalculateMovement();

            stateMachine.WeaponLogic.SetAttack(stateMachine.CurrentWeapon.Damage, stateMachine.CurrentWeapon.Knockbacks[attackIndex]);

            stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        }

        public override void Update(float deltaTime)
        {
            Move(deltaTime);
            FaceOnTarget(deltaTime);

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

            if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {

                if (normalizedTime >= attack.ForceTime)
                {
                    TryApplyForce();
                }

                if (stateMachine.InputManager.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else
            {
                ReturnToLocomotion();
            }

            previousFrameTime = normalizedTime;

            if (direction != Vector3.zero)
                FaceMovementDirection(direction, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputManager.TargetEvent -= OnTarget;

            /*stateMachine.RightHandHolder.transform.GetChild(0).transform.localRotation =
                Quaternion.Euler(0, 90f, 0);*/
        }

        private void TryApplyForce()
        {
            if (alreadyAppliedForce) { return; }

            if (direction == Vector3.zero || stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
            }
            else
            {
                stateMachine.ForceReceiver.AddForce(direction * attack.Force);
            }

            alreadyAppliedForce = true;
        }

        private void TryComboAttack(float normalizedTime)
        {
            if (attack.ComboStateIndex == -1) { return; }

            if (normalizedTime < attack.ComboAttackTime) { return; }

            stateMachine.SwitchState
                (
                new PlayerMeleeAttackState(
                    stateMachine,
                    attack.ComboStateIndex,
                    direction
                    )
                );
        }

        private void OnTarget()
        {
            if (!stateMachine.Targeter.SelectTarget()) { return; }

            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }
}
