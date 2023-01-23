using Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class InputCheckSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<MoveInputComponent> _pool;
        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<PlayerComponent>().Inc<MoveInputComponent>().End();
            _pool = systems.GetWorld().GetPool<MoveInputComponent>();   
        }

        public void Run(IEcsSystems systems)
        {
            var direction = Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction.x = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction.y = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction.x = -1;
            }
            // if(direction != Vector2.zero)
            //     Debug.Log($"Input: {direction}");
 
            foreach (var entity in _filter)
            {
                ref var input = ref _pool.Get(entity);
                input.Value = direction;
            }
        }

     
    }
}