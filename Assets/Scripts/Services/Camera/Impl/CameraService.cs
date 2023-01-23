namespace Services.Camera.Impl
{
    public class CameraService : ICameraService
    {
        private UnityEngine.Camera _mainCamera;

        public UnityEngine.Camera mainCamera
        {
            get => _mainCamera;
            set => _mainCamera = value;
        }
    }
}