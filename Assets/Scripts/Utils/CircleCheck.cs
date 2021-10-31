using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    public class CircleCheck : MonoBehaviour
    {
        [SerializeField]
        private float radius = 0.05f;

        [SerializeField]
        private LayerMask collisionMask;

        [SerializeField]
        private int id = 0;

        public bool Check()
        {
            return Physics2D.OverlapCircle(transform.position, radius, collisionMask);
        }

        public int GetId()
        {
            return id;
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}
