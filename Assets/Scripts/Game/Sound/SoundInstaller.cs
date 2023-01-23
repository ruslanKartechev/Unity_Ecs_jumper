using Game.Sound.Data;
using Game.Sound.Impl;
using UnityEngine;
using Zenject;

namespace Game.Sound
{
    public class SoundInstaller : MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private SoundRepository _soundRepository;
        [SerializeField] private GlobalSoundSettings globalSoundSettings;
        [SerializeField] private AudioSourceProvider _audioSourceProvider;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundRepository>().FromInstance(_soundRepository);
            Container.BindInterfacesAndSelfTo<GlobalSoundSettings>().FromInstance(globalSoundSettings);
            Container.BindInterfacesAndSelfTo<AudioSourceProvider>().FromInstance(_audioSourceProvider);
            Container.BindInterfacesAndSelfTo<SoundManager>().FromInstance(_soundManager);
            
        }
    }
}