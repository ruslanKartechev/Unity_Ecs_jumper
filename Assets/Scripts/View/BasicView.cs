using UnityEngine;

namespace View
{
    public class BasicView : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Renderer _renderer;
        
        public Transform mTransform => transform;
        public Collider collider => _collider;
        public Vector3 position => transform.position;
        public Quaternion rotation => transform.rotation;
        public Vector3 velocity => _rb.velocity;
        public Material material => _renderer.material;
        
        
    }
}