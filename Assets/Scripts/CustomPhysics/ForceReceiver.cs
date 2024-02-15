using UnityEngine;
using UnityEngine.AI;

namespace HeroicQuest.CustomPhysics
{
    /// <summary>
    /// ForceReceiver gestice forze (come il Knockback o il Dash) e gravità su componenti come il CharacterController
    /// e il NavMeshAgent dove non è possibile applicare delle forze tramite soluzioni in engine di Unity
    /// La classe è implementabile sia per il Player che utilizza il CharacterController o la IA che utilizza il NavMeshAgent
    /// </summary>
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float drag = 0.3f;

        private Vector3 dampingVelocity;
        private Vector3 impact;
        private float verticalVelocity;

        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        private void Awake()
        {
            if (TryGetComponent(out CharacterController controller))
            {
                this.controller = controller;
            }

            if (TryGetComponent(out NavMeshAgent agent))
            {
                this.agent = agent;
            }
        }

        private void Update()
        {
            if (verticalVelocity < 0f && controller.isGrounded)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

            if (agent != null)
            {
                if (impact.sqrMagnitude < 0.2f * 0.2f)
                {
                    impact = Vector3.zero;
                    agent.enabled = true;
                }
            }
        }

        public void Reset()
        {
            impact = Vector3.zero;
            verticalVelocity = 0f;
        }

        public void AddForce(Vector3 force)
        {
            impact += force;

            if (agent != null)
            {
                agent.enabled = false;
            }
        }
    }
}