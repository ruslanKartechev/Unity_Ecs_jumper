using UnityEngine;

namespace Services.Raycast
{
    public interface IRaycastService
    {
        public RaycastHit CastFromScreen(Vector2 screenPos, LayerMask layerMask);
        public RaycastHit CastFromPoint(Vector3 origin, Vector3 direction, LayerMask mask);
    }
}
