using UnityEngine;
using TheNecromancers.Managers;
using TheNecromancers.Combat;
using TheNecromancers.CustomPhysics;
using TheNecromancers.Data;
using TheNecromancers.Gameplay.Player;

namespace TheNecromancers.StateMachine.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(ForceReceiver))]
    [RequireComponent(typeof(InteractionDetector))]
    public class PlayerStateMachine : StateMachine
    {
        [field: Header("Components")]
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public InteractionDetector InteractionDetector { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float RollForce { get; private set; }
        [field: SerializeField] public float RollDuration { get; private set; }

        [field: Header("Attacking Settings")]
        [field: SerializeField] public WeaponLogic WeaponLogic { get; private set; }
        [field: SerializeField] public Transform RightHandHolder { get; private set; }
        [field: SerializeField] public Transform LeftHandHolder { get; private set; }
        public Attack[] Attacks { get; set; }

        [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }

        public Transform MainCameraTransform { get; private set; }


        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();
            ForceReceiver = GetComponent<ForceReceiver>();
            InteractionDetector = GetComponent<InteractionDetector>();
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
            }

            if (RightHandHolder)
            {
                WeaponLogic = RightHandHolder.transform.GetComponentInChildren<WeaponLogic>();
            }
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        //Animations Events
        void OnStartAttackAnim()
        {
        }

        void Hit()
        {
        }

        void OnEndAttackAnim()
        {
        }

        void FootR()
        {

        }

        void FootL()
        {

        }
    }
}