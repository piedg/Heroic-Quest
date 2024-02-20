using HeroicQuest.CustomPhysics;
using HeroicQuest.Data;
using HeroicQuest.Gameplay.Combat.Attack;
using HeroicQuest.Gameplay.Combat.Targeting;
using HeroicQuest.Gameplay.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace HeroicQuest.StateMachine.Enemy
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ForceReceiver))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class EnemyStateMachine : StateMachine
    {
        [field: Header("Enemy Settings")]
        [field: SerializeField] public EnemySO EnemySO { get; private set; }
        [field: Header("Components")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        // [field: SerializeField] public CooldownManager CooldownManager { get; private set; }
        // [field: SerializeField] public ParticleFXManager ParticleFXManager { get; private set; }
        // [field: SerializeField] public EnemyPresenter EnemyPresenter { get; private set; }
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
        [field: SerializeField] public Transform RightHandHolder { get; private set; }
        [field: SerializeField] public float StunDuration { get; private set; }

        [field: SerializeField] public GameObject Player { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            Health = GetComponent<Health>();
            Agent = GetComponent<NavMeshAgent>();

            Player = GameObject.Find("Player");

            EnemySO.SpawnModel(gameObject.transform);
            Animator.Rebind(); // After spawning the character prefab it can be animated
            transform
            RightHandHolder = transform.GetChild(0).Find("Root/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
            EnemySO.WeaponSO.Equip(RightHandHolder, Animator);
            WeaponLogic = RightHandHolder.GetComponentInChildren<WeaponLogic>();


            Health.OnDie += OnDie;
        }

        private void OnDestroy()
        {
            Health.OnDie -= OnDie;
        }

        private void Start()
        {
            SwitchState(new EnemyIdleState(this));
        }

        private void OnDie()
        {
            SwitchState(new EnemyDeadState(this));
        }
    }
}