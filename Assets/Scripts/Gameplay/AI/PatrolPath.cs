using UnityEngine;

namespace HeroicQuest.Gameplay.AI
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.5f;

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount) { return 0; };

            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }
    }
}
