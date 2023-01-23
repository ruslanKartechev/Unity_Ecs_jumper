using Game;
using Game.Level.Impl;
using Game.Sound.Impl;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ManagersInstaller : MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private AudioSourceProvider _audioSourceProvider;
        

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundManager>().FromInstance(_soundManager).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioSourceProvider>().FromInstance(_audioSourceProvider).AsSingle();

            Container.BindInterfacesAndSelfTo<WindowsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
        
    }
}