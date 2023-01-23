using System.Collections;
using Leopotam.EcsLite;
using Services.MonoHelpers;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class SystemRunner
    {
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;
        private EcsSystems _initSystems;

        [Inject] private ICoroutineService _coroutine;

        private Coroutine _updating;
        private Coroutine _fixedUpdating;
        
        
        public void AddInit(EcsSystems systems)
        {
            _initSystems = systems;
        }
        
        public void AddUpdate(EcsSystems systems)
        {
            _updateSystems = systems;

        }
        
        public void AddFixedUpdate(EcsSystems systems)
        {
            _fixedUpdateSystems = systems;
        }
        
        public void AddLateUpdate(EcsSystems systems)
        {
            _lateUpdateSystems = systems;
        }

        public void RunInit()
        {
            _initSystems.Init();
            _initSystems.Run();
        }

        public void StartRunning()
        {
            _updateSystems.Init();
            _fixedUpdateSystems.Init();
            
            _updating = _coroutine.StartCor(Running());
            _fixedUpdating = _coroutine.StartCor(FixedRunning());
        }

        public void StopRunning()
        {
            if(_updating != null)
                _coroutine.StopCor(_updating);
            if(_fixedUpdating != null)
                _coroutine.StopCor(_fixedUpdating);
        }
        

        public IEnumerator Running()
        {
            while (true)
            {
                _updateSystems.Run();
                yield return null;
            }
        }

        public IEnumerator FixedRunning()
        {
            while (true)
            {
                _fixedUpdateSystems.Run();
                yield return new WaitForFixedUpdate();
            }   
        }
        
        
    }
}