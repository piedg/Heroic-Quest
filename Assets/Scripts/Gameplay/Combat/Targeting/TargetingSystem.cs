using System;
using UnityEngine;


namespace HeroicQuest.Gameplay.Combat.Targeting
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] float DetectionRange = 5f;
        [SerializeField] Transform TargetIndicator;

        Collider[] enemies;

        private Transform currentTarget;
        public Transform CurrentTarget { get => currentTarget; }
        int currentIndex;

        void Update()
        {

            enemies = Physics.OverlapSphere(transform.position, DetectionRange, LayerMask.GetMask("Enemy"));

            if (enemies.Length <= 0)
            {
                Cancel();
            }

            UpdateTargetIndicator();
        }

        private void UpdateTargetIndicator()
        {
            if (currentTarget)
            {
                TargetIndicator.gameObject.SetActive(true);
                TargetIndicator.position = currentTarget.position + new Vector3(0, 2f, 0f);
            }
            else
            {
                TargetIndicator.gameObject.SetActive(false);
            }
        }

        public bool SelectTarget()
        {
            currentTarget = FindClosestEnemy(enemies);
            return currentTarget != null;
        }

        public void Cancel()
        {
            if (currentTarget == null) { return; }
            currentTarget = null;
        }

        private Transform FindClosestEnemy(Collider[] colliders)
        {
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];

                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    currentIndex = i;
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
            return closestEnemy;
        }

        public void NextTarget()
        {
            currentIndex = (currentIndex + 1) % enemies.Length;
            currentTarget = enemies[currentIndex].transform;
        }

        public void PrevTarget()
        {
            currentIndex = (currentIndex - 1 + enemies.Length) % enemies.Length;
            currentTarget = enemies[currentIndex].transform;
        }

        public void ShowIndicator()
        {
            TargetIndicator.gameObject.SetActive(true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DetectionRange);
        }
    }
}