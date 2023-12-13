using TheNecromancers.CustomPhysics;
using TheNecromancers.Data;
using TheNecromancers.Gameplay.Combat.Attack;
using TheNecromancers.Gameplay.Combat.Targeting;
using UnityEngine;
using UnityEngine.AI;

namespace TheNecromancers.StateMachine.Enemy
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ForceReceiver))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Target))]
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        // [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        // [field: SerializeField] public CooldownManager CooldownManager { get; private set; }
        // [field: SerializeField] public ParticleFXManager ParticleFXManager { get; private set; }
        // [field: SerializeField] public EnemyPresenter EnemyPresenter { get; private set; }
        [field: SerializeField] public Target Target { get; private set; }
        [field: Header("Movement")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }

        [field: Header("Chasing And Patrolling")]
        [field: SerializeField] public float PlayerChasingRange { get; private set; }
        [field: SerializeField] public float PlayerToNearChasingRange { get; private set; }
        [field: SerializeField] public float ViewAngle { get; private set; }
        [field: SerializeField] public float SuspicionTime { get; private set; }
        [field: SerializeField] public float DwellTime { get; private set; }
        // [field: SerializeField] public PatrolPath PatrolPath { get; private set; }
        [field: SerializeField] public LayerMask PlayerLayerMask { get; private set; }
        [field: Header("Attack")]
        [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackRate { get; private set; }
        [field: SerializeField] public float AttackForce { get; private set; }
        //  [field: SerializeField] public Transform SlashVFX { get; private set; }
        public WeaponLogic WeaponLogic { get; private set; }
        [field: SerializeField] public Transform ProjectileObj { get; private set; }
        [field: SerializeField] public bool IsRanged { get; private set; }

        // [field: SerializeField] public int AttackDamage { get; private set; }
        // [field: SerializeField] public float AttackKnockback { get; private set; }
        [field: SerializeField] public GameObject RightHandHolder { get; private set; }
        [field: SerializeField] public float StunDuration { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();
            ForceReceiver = GetComponent<ForceReceiver>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GetComponent<Target>();
        }
    }
}