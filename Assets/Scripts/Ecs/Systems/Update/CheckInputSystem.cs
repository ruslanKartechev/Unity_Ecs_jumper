using Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class CheckInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<MoveInputComponent> _inputPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = systems.GetWorld().Filter<PlayerComponent>().End();
            _inputPool = systems.GetWorld().GetPool<MoveInputComponent>();   
        }

        public void Run(IEcsSystems systems)
        {
            
            var direction = Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction.y = 1;
                AddMove();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction.x = 1;
                AddMove();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction.y = -1;
                AddMove();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction.x = -1;
                AddMove();
            }

            void AddMove()
            {
                foreach (var entity in _filter)
                {
                    if (!_world.HasComponent<MoveInputComponent>(entity))
                    {
                        _inputPool.Add(entity);
                    }
                    ref var inputComp = ref _inputPool.Get(entity);
                    inputComp.Value = direction;
                }
            }
         
        }

     
    }
}