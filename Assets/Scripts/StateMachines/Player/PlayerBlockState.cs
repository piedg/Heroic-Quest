using TheNecromancers.StateMachine.Player;
using UnityEngine;

public class PlayerBlockState : PlayerMovementState
{
    private readonly int BlockHash = Animator.StringToHash("Block");

    private const float crossFadeDuration = 0.1f;
    private float remainingBlockTime = 0.65f;

    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, crossFadeDuration);

    }

    public override void Update(float deltaTime)
    {
        Move(deltaTime);

        remainingBlockTime -= deltaTime;

        if (remainingBlockTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
            return;
        }
    }
    public override void Exit() { }

}
