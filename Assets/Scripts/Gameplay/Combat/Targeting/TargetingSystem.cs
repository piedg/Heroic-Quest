using HeroicQuest.Gameplay.Combat.Targeting;
using System;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] float DetectionRange = 5f;
    [SerializeField] Transform TargetIndicator;

    Collider[] targets;

    private Transform currentTarget;
    public Transform CurrentTarget { get => currentTarget; }

    void Update()
    {
        // Trova tutti gli oggetti con il tag "Enemy" nell'area di rilevamento
        targets = Physics.OverlapSphere(transform.position, DetectionRange, LayerMask.GetMask("Enemy"));

        if (currentTarget != null && !IsEnemyInColliders(currentTarget, targets))
        {
            currentTarget = FindClosestEnemy(targets);
        }

        if (targets.Length <= 0)
        {
            Cancel();
        }

        RemoveEnemiesOutsideSphere(targets);

        if (currentTarget != null)
        {
            TargetIndicator.position = currentTarget.position;
        }
        else
        {
            TargetIndicator.gameObject.SetActive(false);
        }
    }

    public bool HasTarget()
    {
        currentTarget = FindClosestEnemy(targets);
        return currentTarget != null;
    }

    public void Cancel()
    {
        if (currentTarget == null) { return; }
        currentTarget = null;
    }

    private void RemoveEnemiesOutsideSphere(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Target target))
            {
                if (!IsEnemyInColliders(target.transform, colliders))
                {
                    colliders[i] = null;
                }
            }
        }
    }

    private bool IsEnemyInColliders(Transform enemyTransform, Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.transform == enemyTransform)
            {
                return true;
            }
        }
        return false;
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
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }
        return closestEnemy;
    }

    public void NextTarget()
    {
        /* int nextIndex = (currentTargetIndex + 1);
         if (nextIndex == targets.Length - 1)
         {
             currentTarget = targets[0].transform;
             return;
         }
         currentTarget = targets[nextIndex].transform; */
    }

    public void PrevTarget()
    {
        /* if (currentTargetIndex == 0)
         {
             currentTarget = targets[targets.Length - 1].transform;
             return;
         }

         int previousIndex = (currentTargetIndex - 1);
         currentTarget = targets[previousIndex].transform; */
    }

    public void ShowIndicator()
    {
        TargetIndicator.gameObject.SetActive(true);
    }
}
