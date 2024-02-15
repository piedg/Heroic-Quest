using System.Collections.Generic;
using UnityEngine;

namespace HeroicQuest.Gameplay.Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private Transform TargetIndicator;

        private Camera mainCamera;
        private SphereCollider sphereCollider;
        private List<Target> targets = new List<Target>();

        private Target currentTarget;
        public Target CurrentTarget { get => currentTarget; }
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
            if (currentTarget != null)
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

            RemoveTarget(target);
        }

        Transform FindClosestEnemy(GameObject[] enemies)
        {
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance && distance <= DetectionRange)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            return closestEnemy;
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

            currentTarget = closestTarget;
            SetTargetIndicator(currentTarget);

            return true;
        }

        public void Cancel()
        {
            if (currentTarget == null) { return; }
            currentTarget = null;
        }

        private void RemoveTarget(Target target)
        {
            if (currentTarget == target)
            {
                currentTarget = null;
            }

            target.OnDestroyed -= RemoveTarget;
            targets.Remove(target);

            SetTargetIndicator(currentTarget);
        }

        public void NextTarget()
        {
            if (targets.IndexOf(currentTarget) + 1 == targets.Count)
            {
                currentTarget = targets[0];
            }
            else
            {
                currentTarget = targets[targets.IndexOf(currentTarget) + 1];
            }

            SetTargetIndicator(currentTarget);
        }

        public void PrevTarget()
        {
            if (targets.IndexOf(currentTarget) == 0)
            {
                currentTarget = targets[targets.Count - 1];
            }
            else
            {
                currentTarget = targets[targets.IndexOf(currentTarget) - 1];
            }

            SetTargetIndicator(currentTarget);
        }

        private void SetTargetIndicator(Target currentTarget)
        {
            currentTargetTransform = currentTarget?.GetComponent<Transform>();
        }

        public void ShowIndicator()
        {
            TargetIndicator.gameObject.SetActive(true);
        }
    }
}