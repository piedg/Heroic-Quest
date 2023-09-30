using TheNecromancers.Managers;
using UnityEngine;

namespace TheNecromancers.StateMachine.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InputManager))]
    public class PlayerStateMachine : StateMachine
    {
        [field: Header("Controllers")]
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float RollForce { get; private set; }
        [field: SerializeField] public float RollDuration { get; private set; }

        public Transform MainCameraTransform { get; private set; }


        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;

            SwitchState(new PlayerLocomotionState(this));
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

        void OnHitAnim()
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