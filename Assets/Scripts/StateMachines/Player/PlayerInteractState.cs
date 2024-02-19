using HeroicQuest.Data;
using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerInteractState : PlayerBaseState
    {
        private readonly int interactHash = Animator.StringToHash("Interact");
        private const float crossFadeDuration = 0.1f;


        public PlayerInteractState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(interactHash, crossFadeDuration);
            stateMachine.WeaponLogic.gameObject.SetActive(false);

            stateMachine.InteractionDetector.CurrentTarget.Interact();

            PickuppableItem item = stateMachine.InteractionDetector.CurrentTarget as PickuppableItem;
            WeaponSO newWeapon = item.GetWeapon();
            stateMachine.SetCurrentWeapon(newWeapon);
            newWeapon.Equip(stateMachine.RightHandHolder, stateMachine.Animator);
            stateMachine.FindWeaponLogic();
        }

        public override void Update(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Interact");
            if (normalizedTime > 1f)
            {
                ReturnToLocomotion();
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.WeaponLogic.gameObject.SetActive(true);
            stateMachine.InteractionDetector.CurrentTarget = null;
        }
    }
}