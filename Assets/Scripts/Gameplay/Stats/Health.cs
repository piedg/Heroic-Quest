using System;
using UnityEngine;

namespace HeroicQuest.Gameplay.Stats
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int MaxHealth = 100;

        private int currentHealth;
        public bool IsDead => currentHealth == 0;

        public event Action OnTakeDamage;
        public event Action OnDie;

        private void Start()
        {
            currentHealth = MaxHealth;
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
            currentHealth = MaxHealth;
        }
    }
}
