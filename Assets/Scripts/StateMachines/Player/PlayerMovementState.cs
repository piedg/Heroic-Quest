using UnityEngine;
using HeroicQuest.StateMachine.Player;

public abstract class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.Controller.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputManager.MovementValue.y + right * stateMachine.InputManager.MovementValue.x;
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationSpeed);
    }

    protected void FaceOnTarget(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(lookPos),
            deltaTime * stateMachine.RotationSpeed);
    }
}
