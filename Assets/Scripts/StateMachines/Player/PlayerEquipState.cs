using UnityEngine;

namespace HeroicQuest.StateMachine.Player
{
    public class PlayerEquipState : PlayerBaseState
    {
        private readonly int equipHash = Animator.StringToHash("Equip");

        private const float crossFadeDuration = 0.1f;

        public PlayerEquipState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(equipHash, crossFadeDuration);
            //stateMachine.WeaponLogic.gameObject.SetActive(false); // At least a weapon should be equipped (Default Unarmed)
        }

        public override void Update(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Equip");
            Debug.Log("NormalizedTime " + normalizedTime);
            if (normalizedTime > 0.45f)
            {
                ReturnToLocomotion();
                return;
            }
        }

        public override void Exit()
        {
            //    stateMachine.WeaponLogic.gameObject.SetActive(true);
        }
    }
}