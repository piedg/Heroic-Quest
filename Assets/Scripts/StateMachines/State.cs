using UnityEngine;

namespace HeroicQuest.StateMachine
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Update(float deltaTime);
        public abstract void Exit();

        protected float GetNormalizedTime(Animator animator, string tag)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }
            else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }

        protected bool IsPlayingAnimation(Animator animator)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            return (animator.IsInTransition(0) || currentInfo.normalizedTime < 1f);
        }

        protected bool IsPlayingAnimation(Animator animator, string tag)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            return (animator.IsInTransition(0) && currentInfo.IsTag(tag)) || (currentInfo.normalizedTime) < 1f;
        }

        protected bool CheckDistanceSqr(Vector3 A, Vector3 B, float accuracy)
        {
            float distanceSqr = (A - B).sqrMagnitude;
            return distanceSqr <= accuracy * accuracy;
        }
    }
}