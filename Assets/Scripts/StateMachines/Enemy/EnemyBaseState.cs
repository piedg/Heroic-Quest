using UnityEngine;
using HeroicQuest.Gameplay.Stats;
using HeroicQuest.StateMachine.Player;
using UnityEngine.AI;

namespace HeroicQuest.StateMachine.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// Add a movement to the CharacterController
        /// </summary>
        private void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        /// <summary>
        /// The Character doesn't move
        /// </summary>
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        private void AgentMoveTo(Vector3 position, float speed, float deltaTime)
        {
            Move(deltaTime);

            if (stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = position;

                Move(stateMachine.Agent.desiredVelocity.normalized * speed, deltaTime);
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }

        /// <summary>
        /// Move the Agent to the Player
        /// </summary>
        protected void MoveToPlayer(float deltaTime)
        {
            AgentMoveTo(stateMachine.Player.transform.position, stateMachine.MovementSpeed, deltaTime);
        }

        /// <summary>
        /// Move the Character to a specific position
        /// </summary>
        protected void MoveTo(Vector3 position, float speed, float deltaTime)
        {
            AgentMoveTo(position, speed, deltaTime);
        }

        private void SmoothRotation(Vector3 target, float deltaTime)
        {
            Vector3 lookPos = target - stateMachine.transform.position;
            lookPos.y = 0f;

            var targetRotation = Quaternion.LookRotation(lookPos);

            stateMachine.transform.rotation = Quaternion.Slerp(
                stateMachine.transform.rotation,
                targetRotation,
                stateMachine.RotationSpeed * deltaTime);
        }

        /// <summary>
        /// Rotate the Character smoothly to the Player
        /// </summary>
        protected void FaceToPlayer(float deltaTime)
        {
            if (stateMachine.Player == null) { return; }

            SmoothRotation(stateMachine.Player.transform.position, deltaTime);
        }

        /// <summary>
        /// Rotate the Character smoothly to a specific position
        /// </summary>
        protected void FaceTo(Vector3 position, float deltaTime)
        {
            SmoothRotation(position, deltaTime);
        }

        protected void FaceForward(float deltaTime)
        {
            if (stateMachine.Agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                var targetRotation = Quaternion.LookRotation(stateMachine.Agent.velocity.normalized);

                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, stateMachine.RotationSpeed * deltaTime);
            }
        }

        protected bool IsInAttackRange()
        {
            if (stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

            return CheckDistanceSqr(stateMachine.Player.transform.position, stateMachine.transform.position, stateMachine.AttackRange);
        }

        protected bool IsInViewRange()
        {
            Vector3 disToPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;

            Vector3 localDirection = stateMachine.transform.InverseTransformDirection(disToPlayer);

            Debug.DrawRay(stateMachine.transform.position + Vector3.up, disToPlayer + Vector3.up, Color.red);

            float angle = (Mathf.Atan2(localDirection.z, localDirection.x) * Mathf.Rad2Deg) - 90f;

            if (angle < stateMachine.ViewAngle && angle > -stateMachine.ViewAngle)
            {
                if (Physics.Raycast(stateMachine.transform.position + (Vector3.up / 2), disToPlayer + Vector3.up, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.TryGetComponent(out PlayerStateMachine Player))
                    {
                        return Player != null;
                    }
                }
            }
            return false;
        }

        protected bool IsInChaseRange()
        {
            if (stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

            return CheckDistanceSqr(stateMachine.Player.transform.position, stateMachine.transform.position, stateMachine.PlayerChasingRange);
        }

        protected bool IsPlayerNear()
        {
            if (stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

            return CheckDistanceSqr(stateMachine.Player.transform.position, stateMachine.transform.position, stateMachine.PlayerNearChasingRange);
        }

        protected void ResetAgentPath()
        {
            Move(0); // Avoid NavAgent Path error

            stateMachine.Agent.velocity = Vector3.zero;

            if (stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }
        }

        protected void OnTakeDamage()
        {
            stateMachine.SwitchState(new EnemyHitState(stateMachine));
        }
    }
}
