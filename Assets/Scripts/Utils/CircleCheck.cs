using Interfaces;
using UnityEngine;

namespace Utils
{
    public class CircleCheck : MonoBehaviour, ICheck
    {
        [SerializeField]
        private float radius = 0.05f;

        [SerializeField]
        private LayerMask collisionMask;

        [SerializeField]
        private int id;

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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
