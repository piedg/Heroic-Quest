using System;
using UnityEngine;

namespace HeroicQuest.Gameplay.Stats
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] float invulnerabilityTime = 0.25f;
        public float InvulnerabilityTime => invulnerabilityTime;

        private int currentHealth;
        public bool IsDead => currentHealth == 0;

        public event Action OnTakeDamage;
        public event Action OnDie;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void DealDamage(int damage)
        {
            if (currentHealth == 0) { return; }

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            if (currentHealth == 0)
            {
                OnDie?.Invoke();
            }

            // play FX
            OnTakeDamage?.Invoke();

            Debug.Log(gameObject.name + " Current health " + currentHealth + " damage received " + damage);
        }

        public void RestoreLife()
        {
            currentHealth = maxHealth;
        }
    }
}
