using System.Collections.Generic;
using UnityEngine;

namespace TheNecromancers.Gameplay.Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private Transform TargetIndicator;

        private Camera mainCamera;
        private SphereCollider sphereCollider;
        private List<Target> targets = new List<Target>();

        public Target CurrentTarget { get; private set; }
        private Transform currentTargetTransform;

        [field: SerializeField] public float DetectionRange { get; private set; }


        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            mainCamera = Camera.main;

            sphereCollider.radius = DetectionRange;
            sphereCollider.isTrigger = true;

            TargetIndicator.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (CurrentTarget != null)
            {
                if (currentTargetTransform != null)
                {
                    TargetIndicator.position = currentTargetTransform.position;
                }
            }
            else
            {
                TargetIndicator.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Target target)) { return; }

            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Target target)) { return; }
            //if (CurrentTarget == null) { return; }

            // questo risolve il fatto che rollando senza essere dentro il targeting viene lockato un nemico
            // ma produce un bug per cui quando sei loccato ed esci dal trigger continua ad essere lockato
            /*if (!other.TryGetComponent(out Target target)) { return; }*/
            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if (targets.Count == 0) { return false; }

            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Target target in targets)
            {
                Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

                if (!target.GetComponentInChildren<Renderer>().isVisible)
                {
                    continue;
                }

                Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
                if (toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }

            if (closestTarget == null) { return false; }

            CurrentTarget = closestTarget;
            SetTargetIndicator(CurrentTarget);

            return true;
        }

        public void Cancel()
        {
            if (CurrentTarget == null) { return; }
            CurrentTarget = null;
        }

        private void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                CurrentTarget = null;
            }

            target.OnDestroyed -= RemoveTarget;
            targets.Remove(target);

            if (targets.Count > 0)
            {
                CurrentTarget = targets[0];
            }

            SetTargetIndicator(CurrentTarget);
        }

        public void NextTarget()
        {
            if (targets.IndexOf(CurrentTarget) + 1 == targets.Count)
            {
                CurrentTarget = targets[0];
            }
            else
            {
                CurrentTarget = targets[targets.IndexOf(CurrentTarget) + 1];
            }

            SetTargetIndicator(CurrentTarget);
        }

        public void PrevTarget()
        {
            if (targets.IndexOf(CurrentTarget) == 0)
            {
                CurrentTarget = targets[targets.Count - 1];
            }
            else
            {
                CurrentTarget = targets[targets.IndexOf(CurrentTarget) - 1];
            }

            SetTargetIndicator(CurrentTarget);
        }

        private void SetTargetIndicator(Target CurrentTarget)
        {
            currentTargetTransform = CurrentTarget?.GetComponent<Transform>();
        }

        public void ShowIndicator()
        {
            TargetIndicator.gameObject.SetActive(true);
        }
    }
}