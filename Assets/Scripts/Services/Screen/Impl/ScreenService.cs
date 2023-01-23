using UnityEngine;

namespace Services.Screen.Impl
{
    public class ScreenService: IScreenService
    {
        private UnityEngine.Camera _camera;
        private int _width;        
        private int _height;        

        
        public ScreenService()
        {
            _camera = UnityEngine.Camera.main;
            _width = UnityEngine.Screen.width;
            _height = UnityEngine.Screen.height;

        }
        
        public bool IsOnScreen(Vector3 worldPosition, float screenOffsetPercent)
        {
            var screenPosition = _camera.WorldToScreenPoint(worldPosition);
            if (screenPosition.x > _width * screenOffsetPercent
                && screenPosition.x < _width * (1 - screenOffsetPercent) 
                && screenPosition.y > _height * screenOffsetPercent
                && screenPosition.y < _height * (1 - screenOffsetPercent)
                )
            {
                return true;
            }

            return false;
        }
    }
}