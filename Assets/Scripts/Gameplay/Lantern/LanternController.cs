using TheNecromancers.StateMachine.Player;
using UnityEngine;

namespace TheNecromancers.Gameplay.Lantern
{

    public class LanternController : MonoBehaviour
    {
        public Transform player; // Trascina il transform del giocatore nell'Inspector
        public float smoothSpeed; // Regola la velocità di movimento
        public float collisionDistance; // Distanza di sicurezza per evitare collisioni
        public float offsetUp; // Distanza con il player verso l'alto
        public float offsetRight; // Distanza con il player verso l'alto
        public LayerMask EnvironmentLayer; // Distanza con il player verso l'alto

        // Oscillation
        public float oscillationSpeed; // Velocità dell'oscillazione
        public float oscillationHeight; // Altezza massima dell'oscillazione

        float timer = 0f;

        private void Awake()
        {
            player = FindObjectOfType<PlayerStateMachine>().transform;
        }

        void Update()
        {
            // Incrementa il timer basato sul tempo trascorso
            timer += Time.deltaTime;

            // Calcola l'offset verticale basato sulla funzione sinusoidale
            float verticalOffset = Mathf.Sin(timer * oscillationSpeed) * oscillationHeight;

            // Calcola la posizione desiderata della lanterna (ad esempio, dietro il giocatore)
            Vector3 targetPosition = player.position - (-player.up * (offsetUp + verticalOffset) + -player.right * offsetRight);

            RaycastHit hit;
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);

            if (Physics.BoxCast(transform.position, halfExtents, transform.forward, out hit, transform.rotation, collisionDistance, EnvironmentLayer))
            {
                // Calcola la direzione opposta alla normale della collisione
                Vector3 avoidDirection = -hit.normal;

                // Aggiorna la posizione desiderata della lanterna
                targetPosition = hit.point + avoidDirection * collisionDistance;
            }

            // Muovi la lanterna verso la posizione desiderata in modo fluido
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        }
    }
}