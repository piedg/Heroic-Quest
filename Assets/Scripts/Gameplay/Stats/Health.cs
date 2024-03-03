using HeroQuest.Saving;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace HeroicQuest.Gameplay.Stats
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] float invulnerabilityTime = 0.25f;
        public float InvulnerabilityTime => invulnerabilityTime;

        private int currentHealth;
        public bool IsDead => currentHealth == 0;

        public event Action OnTakeDamage;
        public event Action OnDie;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void DealDamage(int damage)
        {
            if (currentHealth == 0) { return; }

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            HandleDie();

            // play FX
            OnTakeDamage?.Invoke();

            Debug.Log(gameObject.name + " Current health " + currentHealth + " damage received " + damage);
        }

        private void HandleDie()
        {
            if (currentHealth == 0)
            {
                OnDie?.Invoke();
            }
        }

        public void RestoreLife()
        {
            currentHealth = maxHealth;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentHealth);
        }

        public void RestoreFromJToken(JToken state)
        {
            currentHealth = state.ToObject<int>();

            if (currentHealth <= 0)
            {
                DealDamage(0);
            }
        }
    }
}
