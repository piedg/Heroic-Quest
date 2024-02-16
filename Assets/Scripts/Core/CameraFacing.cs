using UnityEngine;

namespace HeroicQuest.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
