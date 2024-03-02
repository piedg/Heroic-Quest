using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemyPatrolState : EnemyBaseState
    {
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int speedHash = Animator.StringToHash("Speed");

        private const float crossFadeduration = 0.1f;
        private const float animatorDumpTime = 0.1f;

        int currentWaypointIndex = 0;
        Vector3 nextPosition;
        float dwellTimeElapsed = 0;

        public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(locomotionHash, crossFadeduration);
            currentWaypointIndex = stateMachine.LastWaypointIndex;
        }

        public override void Update(float deltaTime)
        {
            if (IsInChaseRange() &&
                (IsInViewRange() ||
                IsPlayerNear()))
            {
                // stateMachine.EnemyPresenter.ShowExclamationMark();
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            dwellTimeElapsed += deltaTime;

            if (AtWaypoint())
            {
                dwellTimeElapsed = 0;
                CycleWaypoint();
            }

            if (dwellTimeElapsed < stateMachine.DwellTime)
            {
                Move(deltaTime);
                stateMachine.Animator.SetFloat(speedHash, 0f, animatorDumpTime, deltaTime);
                ResetAgentPath();
                return;
            }

            nextPosition = GetCurrentWaypoint();

            FaceForward(deltaTime);
            MoveTo(nextPosition, stateMachine.WalkingSpeed, deltaTime);

            stateMachine.Animator.SetFloat(speedHash, stateMachine.WalkingSpeed, animatorDumpTime, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.LastWaypointIndex = currentWaypointIndex;
            ResetAgentPath();
        }

        private bool AtWaypoint()
        {
            return CheckDistanceSqr(stateMachine.transform.position, GetCurrentWaypoint(), 1f);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return stateMachine.PatrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = stateMachine.PatrolPath.GetNextIndex(currentWaypointIndex);
        }
    }
}
