using System.Collections.Generic;
using UnityEngine;
using HeroicQuest.CustomPhysics;
using HeroicQuest.Gameplay.Stats;
using System.Collections;

namespace HeroicQuest.Gameplay.Combat.Attack
{
    public class WeaponLogic : MonoBehaviour
    {
        [SerializeField] LayerMask layerToInteract;
        private int damage;
        private float knockback;

        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnEnable()
        {
            alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!((layerToInteract.value & (1 << other.transform.gameObject.layer)) > 0)) { return; }

            if (alreadyCollidedWith.Contains(other)) { return; }

            alreadyCollidedWith.Add(other);

            if (other.TryGetComponent(out Health health))
            {
                health.DealDamage(damage);
                StartCoroutine(RemoveCollidersCO(health.InvulnerabilityTime));
            }

            if (other.TryGetComponent(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                direction.y = 0;
                forceReceiver.AddForce(direction * knockback);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            alreadyCollidedWith.Clear();
        }

        public void SetAttack(int damage, float knockback)
        {
            this.damage = damage;
            this.knockback = knockback;
        }

        public void SetAttack(int damage, float knockback, bool isEnemy)
        {
            this.damage = damage;
            this.knockback = knockback;

            if (isEnemy)
            {
                layerToInteract.value = 1 << LayerMask.NameToLayer("Player");
            }
            else
            {
                layerToInteract.value = 1 << LayerMask.NameToLayer("Enemy");
            }
        }

        private IEnumerator RemoveCollidersCO(float invulnerabilityTime)
        {
            yield return new WaitForSeconds(invulnerabilityTime);
            alreadyCollidedWith.Clear();
        }
    }
}