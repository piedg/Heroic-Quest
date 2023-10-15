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
            stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));

            /* if (stateMachine.Targeter.CurrentTarget != null)
             {
                 Debug.Log("Go To Targeting");
                 stateMachine.Targeter.ShowIndicator();
                 stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
             }
             else
             {
                 stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
             }*/
        }
    }
}