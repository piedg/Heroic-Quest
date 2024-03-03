namespace HeroicQuest.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void ReturnToLocomotion()
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
                return;
            }
        }


        protected void OnBlock()
        {
            if (stateMachine.CurrentMainWeapon.IsUnarmed()) return;

            stateMachine.SwitchState(new PlayerBlockState(stateMachine));
        }

        protected void OnInteract()
        {
            if (stateMachine.InteractionDetector.CurrentTarget == null) { return; }

            stateMachine.SwitchState(new PlayerInteractState(stateMachine));
        }

        protected void OnTakeDamage()
        {
            stateMachine.SwitchState(new PlayerHitState(stateMachine));
        }
    }
}