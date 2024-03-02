using UnityEngine;
using HeroicQuest.Managers;
using HeroicQuest.CustomPhysics;
using HeroicQuest.Data;
using HeroicQuest.Gameplay.Interaction;
using HeroicQuest.Gameplay.Combat.Targeting;
using HeroicQuest.Gameplay.Combat.Attack;
using HeroicQuest.Gameplay.Stats;

namespace HeroicQuest.StateMachine.Player
{
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ForceReceiver))]
    [RequireComponent(typeof(InteractionDetector))]
    [RequireComponent(typeof(TargetingSystem))]
    [RequireComponent(typeof(Health))]
    public class PlayerStateMachine : StateMachine
    {
        [field: Header("Components")]
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public InteractionDetector InteractionDetector { get; private set; }
        [field: SerializeField] public TargetingSystem Targeter { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float RollForce { get; private set; }
        [field: SerializeField] public float RollDuration { get; private set; }
        [field: SerializeField] public float RollAnimSpeed { get; private set; }

        [field: Header("Weapon Settings")]
        [field: SerializeField] public WeaponLogic WeaponLogic { get; private set; }
        [field: SerializeField] public Transform RightHandHolder { get; private set; }
        [field: SerializeField] public Transform LeftHandHolder { get; private set; }
        [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }

        public void SetCurrentWeapon(WeaponSO currentWeapon)
        {
            CurrentWeapon = currentWeapon;
        }
        public void FindWeaponLogic()
        {
            WeaponLogic = RightHandHolder.transform.GetComponentInChildren<WeaponLogic>();
        }

        public Transform MainCameraTransform { get; private set; }

        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            ForceReceiver = GetComponent<ForceReceiver>();
            InteractionDetector = GetComponent<InteractionDetector>();
            Targeter = GetComponent<TargetingSystem>();
            Health = GetComponent<Health>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;

            SpawnWeapon();

            SwitchState(new PlayerLocomotionState(this));
        }

        private void SpawnWeapon()
        {
            if (CurrentWeapon)
            {
                CurrentWeapon.Equip(RightHandHolder, Animator);

                if (RightHandHolder)
                {
                    WeaponLogic = RightHandHolder.transform.GetComponentInChildren<WeaponLogic>();
                }
            }
        }

        //Animations Events
        void OnStartAttackAnim()
        {
            WeaponLogic.GetComponent<CapsuleCollider>().enabled = true;
        }

        void Hit()
        {
        }

        void OnEndAttackAnim()
        {
            WeaponLogic.GetComponent<CapsuleCollider>().enabled = false;
        }

        void FootR()
        {

        }

        void FootL()
        {

        }
    }
}