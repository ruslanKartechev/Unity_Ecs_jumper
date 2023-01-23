using Ecs;
using Ecs.Systems;
using Helpers;
using Installers;
using UnityEngine;
using Zenject;

namespace Game
{
    [DefaultExecutionOrder(-1)]
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private SceneContext _sceneContext;
        [SerializeField] private SceneInitializer _sceneInitializer;
        // [SerializeField] private EcsInstaller _ecsInstaller;

        [Inject] private SystemRunner _systemRunner;
        [Inject] private DiContainer _diContainer;
        
        private void Awake()
        {
            InitLogs();
            _sceneContext.Run();
            _sceneInitializer.Run();
        }

        private void Start()
        {
            _systemRunner.RunInit();
            _systemRunner.StartRunning();
        }
        
        private void InitLogs()
        {
            Dbg.EnableDebugs = true;
        }

        
    }
}