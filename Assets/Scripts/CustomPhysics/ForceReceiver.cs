using UnityEngine;
using UnityEngine.AI;

namespace TheNecromancers.CustomPhysics
{
    /// <summary>
    /// ForceReceiver gestice forze (come il Knockback o il Dash) e gravità su componenti come il CharacterController
    /// e il NavMeshAgent dove non è possibile applicare delle forze tramite soluzioni in engine di Unity
    /// La classe è implementabile sia per il Player che utilizza il CharacterController o la IA che utilizza il NavMeshAgent
    /// </summary>
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController Controller;
        [SerializeField] private NavMeshAgent Agent;
        [SerializeField] private float drag = 0.3f;

        private Vector3 dampingVelocity;
        private Vector3 impact;
        private float verticalVelocity;

        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        private void Awake()
        {
            if (TryGetComponent(out CharacterController Controller))
            {
                this.Controller = Controller;
            }

            if (TryGetComponent(out NavMeshAgent Agent))
            {
                this.Agent = Agent;
            }
        }

        private void Update()
        {
            if (verticalVelocity < 0f && Controller.isGrounded)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

            if (Agent != null)
            {
                if (impact.sqrMagnitude < 0.2f * 0.2f)
                {
                    impact = Vector3.zero;
                    Agent.enabled = true;
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

            if (Agent != null)
            {
                Agent.enabled = false;
            }
        }
    }
}