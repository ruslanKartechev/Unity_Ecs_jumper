using UnityEngine;

namespace Data
{
    public struct CollisionData
    {
        public Collider Other;
        public Vector3 RelativeVelocity;
        public ContactPoint[] Contacts;
        public Vector3 Impulse;
            
        public CollisionData(Collision collision)
        {
            Other = collision.collider;
            RelativeVelocity = collision.relativeVelocity;
            Contacts = collision.contacts;
            Impulse = collision.impulse;
        }
    }
}