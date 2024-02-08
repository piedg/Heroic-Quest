using UnityEngine;

namespace TheNecromancers.StateMachine.Player
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
    }
}