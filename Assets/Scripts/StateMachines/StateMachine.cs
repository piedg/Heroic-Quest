using System.Collections;
using UnityEngine;

namespace HeroicQuest.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private State currentState;

        private void Update()
        {
            currentState?.Update(Time.deltaTime);
        }

        public void SwitchState(State newState)
        {
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log(gameObject.name + " Current State: " + currentState?.ToString());
                Debug.Log(gameObject.name + " New State: " + newState.ToString());
            }
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void StartCoroutine(IEnumerable method)
        {
            StartCoroutine(method);
        }
    }
}