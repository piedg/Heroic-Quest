using System.Collections;
using System.Collections.Generic;
using TheNecromancers.StateMachine.Player;
using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.Controller.Move(movement * deltaTime);
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

    public override void Enter() { }

    public override void Update(float deltaTime) { }

    public override void Exit() { }
}
