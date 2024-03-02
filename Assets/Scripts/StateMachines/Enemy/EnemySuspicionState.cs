using UnityEngine;

namespace HeroicQuest.StateMachine.Enemy
{
    public class EnemySuspicionState : EnemyBaseState
    {
        private readonly int speedHash = Animator.StringToHash("Speed");

        private const float animatorDumpTime = 0.1f;

        private float suspicionTime;

        public EnemySuspicionState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            suspicionTime = stateMachine.SuspicionTime;
            //stateMachine.EnemyPresenter.ShowQuestionMark((int)suspicionTime);
        }

        public override void Update(float deltaTime)
        {
            Debug.Log("IsInViewRange() " + IsInViewRange());
            Debug.Log("IsInChaseRange() " + IsInChaseRange());
            if (IsInViewRange() && IsInChaseRange())
            {
                // stateMachine.EnemyPresenter.ShowExclamationMark();
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            suspicionTime -= deltaTime;
            Move(deltaTime);

            if (suspicionTime < 0)
            {
                if (stateMachine.PatrolPath != null)
                {
                    stateMachine.SwitchState(new EnemyPatrolState(stateMachine));
                    return;
                }
                else
                {
                    // back to initial pos
                    FaceForward(deltaTime);
                    MoveTo(stateMachine.InitialPosition, stateMachine.WalkingSpeed, deltaTime);

                    stateMachine.Animator.SetFloat(speedHash, stateMachine.WalkingSpeed, animatorDumpTime, deltaTime);

                    if (CheckDistanceSqr(stateMachine.transform.position, stateMachine.InitialPosition, 1f))
                    {
                        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                        return;
                    }
                }
            }

            stateMachine.Animator.SetFloat(speedHash, 0f, animatorDumpTime, deltaTime);
        }

        public override void Exit()
        {
            ResetAgentPath();
        }
    }
}