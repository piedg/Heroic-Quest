using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack1");
        private const float TransitionDuration = 0.1f;

        float timeBetweenAttacks = 0f;

        private bool alreadyAppliedForce;
        private Vector3 direction;

        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
            stateMachine.Health.OnTakeDamage += OnTakeDamage;
            stateMachine.WeaponLogic.SetAttack(stateMachine.CurrentWeapon.Damage, stateMachine.CurrentWeapon.Knockbacks[0], true);
        }

        public override void Update(float deltaTime)
        {
            Move(deltaTime);
            //FaceToPlayer(deltaTime);
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if (normalizedTime > 0.5f && normalizedTime < 1f)
            {
                TryApplyForce();
                return;
            }

            timeBetweenAttacks += deltaTime;

            if (timeBetweenAttacks < stateMachine.AttackRate)
            {
                FaceToPlayer(deltaTime);
                return;
            }

            timeBetweenAttacks = 0f;

            FaceToPlayer(deltaTime);

            if (normalizedTime < 1) { return; }

            if (normalizedTime >= 1)
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }
        }

        private void TryApplyForce()
        {
            if (alreadyAppliedForce) { return; }

            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * stateMachine.AttackForce);

            alreadyAppliedForce = true;
        }

        public override void Exit()
        {
            stateMachine.Health.OnTakeDamage -= OnTakeDamage;
        }
    }
}