using HeroicQuest.Gameplay.Combat.Targeting;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] float DetectionRange = 5f;

    Collider[] colliders;
    private Transform currentTarget;

    public Transform CurrentTarget { get => currentTarget; }

    void Update()
    {
        // Trova tutti gli oggetti con il tag "Enemy" nell'area di rilevamento
        colliders = Physics.OverlapSphere(transform.position, DetectionRange, LayerMask.GetMask("Enemy"));
        // Rimuovi i nemici che sono fuori dalla sfera
        RemoveEnemiesOutsideSphere(colliders);
        if (currentTarget == null) currentTarget = null;
    }

    public bool SelectTarget()
    {
        currentTarget = FindClosestEnemy(colliders);

        return currentTarget != null;
    }

    void RemoveEnemiesOutsideSphere(Collider[] colliders)
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

    bool IsEnemyInColliders(Transform enemyTransform, Collider[] colliders)
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

    Transform FindClosestEnemy(Collider[] colliders)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }
}
