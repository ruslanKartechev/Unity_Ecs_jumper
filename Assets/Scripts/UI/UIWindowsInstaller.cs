using UI.Views;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIWindowsInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private StartWindowView _startWindowViewPrefab;
        [SerializeField] private ProgressWindowView _progressWindowViewPrefab;
        [SerializeField] private FailWindowView _failWindowViewPrefab;
        [SerializeField] private WinWindowView _winWindowViewPrefab;
        [SerializeField] private BonusWindowView _bonusWindowView;

        [Inject] private WindowsManager _windowsManager;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            var startView = Container.InstantiatePrefabForComponent<StartWindowView>(_startWindowViewPrefab, _parentCanvas.transform);
            var startWindow = new StartWindow(startView);
            Container.Inject(startWindow);
            Container.Bind<StartWindow>().FromInstance(startWindow).AsSingle();
            startWindow.Close();
        

            var progressView = Container.InstantiatePrefabForComponent<ProgressWindowView>(_progressWindowViewPrefab, _parentCanvas.transform);
            var progressWindow = new ProgressWindow(progressView);
            Container.Inject(progressWindow);
            Container.Bind<ProgressWindow>().FromInstance(progressWindow).AsSingle();
            progressWindow.Close();
            
            
            var windowView = Container.InstantiatePrefabForComponent<WinWindowView>(_winWindowViewPrefab, _parentCanvas.transform);
            var winWindow = new WinWindow(windowView);
            Container.Inject(winWindow);
            Container.Bind<WinWindow>().FromInstance(winWindow).AsSingle();
            winWindow.Close();
            
            var failView = Container.InstantiatePrefabForComponent<FailWindowView>(_failWindowViewPrefab, _parentCanvas.transform);
            var failWindow = new FailWindow(failView);
            Container.Inject(failWindow);
            Container.Bind<FailWindow>().FromInstance(failWindow).AsSingle();
            failWindow.Close();
                   
            var bonusWindowView = Container.InstantiatePrefabForComponent<BonusWindowView>(_bonusWindowView, _parentCanvas.transform);
            var bonusWindow = new BonusWindow(bonusWindowView);
            Container.Inject(bonusWindow);
            Container.Bind<BonusWindow>().FromInstance(bonusWindow).AsSingle();
            Container.Bind<IBonusWindow>().FromInstance(bonusWindow).AsSingle();
            bonusWindow.Close();
          
            _windowsManager.Init(startWindow, progressWindow, failWindow, winWindow, bonusWindow);
            _windowsManager.CloseAll();
        }
    }
}