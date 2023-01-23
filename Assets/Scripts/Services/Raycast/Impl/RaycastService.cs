using Services.Camera;
using UnityEngine;

namespace Services.Raycast.Impl
{
    public class RaycastService :  IRaycastService
    {
        private readonly ICameraService _cameraService;
        private float _maxDistance = 250;
        
        public RaycastService(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }
        
        public RaycastHit CastFromScreen(Vector2 screenPos, LayerMask layerMask)
        {
            var ray = _cameraService.mainCamera.ScreenPointToRay(screenPos);
            Physics.Raycast(ray, out RaycastHit hit, _maxDistance, layerMask);
            return hit;
        }

        public RaycastHit CastFromPoint(Vector3 origin, Vector3 direction, LayerMask mask)
        {
            Physics.Raycast(origin, direction, out var hit ,_maxDistance, mask);
            // Debug.DrawLine(origin, origin + direction * 10, Color.green, 10f);
            return hit;
        }
    }
}