using Data.Impl;
using Data.Prefabs.Impl;
using Game.Sound.Data;
using Services.Particles.Service.Impl;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SettingsInstaller : MonoInstaller
    {
        [SerializeField] private GlobalSoundSettings _globalSoundSettings;
        [SerializeField] private SoundRepository _soundRepository;
        [SerializeField] private PrefabsRepository _prefabsRepository;
        [SerializeField] private ParticlesRepository _particlesRepository;
        [SerializeField] private GlobalSettings _globalSettings;
        [SerializeField] private LevelRepository _levelRepository;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PrefabsRepository>().FromInstance(_prefabsRepository).AsSingle();
            Container.BindInterfacesAndSelfTo<ParticlesRepository>().FromInstance(_particlesRepository).AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalSettings>().FromInstance(_globalSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelRepository>().FromInstance(_levelRepository).AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalSoundSettings>().FromInstance(_globalSoundSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<SoundRepository>().FromInstance(_soundRepository).AsSingle();

        }
    }
}