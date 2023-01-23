using Services.Camera.Impl;
using Services.Input.Impl;
using Services.Instantiate.Impl;
using Services.Level.Impl;
using Services.Money.Impl;
using Services.MonoHelpers.Impl;
using Services.Parent.Impl;
using Services.Particles.Service.Impl;
using Services.Prefabs;
using Services.Raycast.Impl;
using Services.Time.Impl;
using Services.UID.Impl;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [Header("Services")] 
        [SerializeField] private PrefsLevelService _levelService;
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private ParticlesService _particlesService;
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UiDGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InstantiateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaycastService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimeService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PrefsLevelService>().FromInstance(_levelService).AsSingle();
            Container.BindInterfacesAndSelfTo<ParentService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CoroutineService>().FromInstance(_coroutineService).AsSingle();
            Container.BindInterfacesAndSelfTo<PrefabsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ParticlesService>().FromInstance(_particlesService).AsSingle();
            Container.BindInterfacesTo<SlowMotionService>().AsSingle();
            
            InstallCamera();
        }

        private void InstallCamera()
        {
            var cameraService = new CameraService();
            cameraService.mainCamera = Camera.main;
            Container.BindInterfacesAndSelfTo<CameraService>().FromInstance(cameraService).AsSingle();
        }
        


    }
}