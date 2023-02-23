using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class ControlSpawnTimeSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<BlocksCountComponent>().Inc<BlockSpawnDelayComponent>().Inc<BlockSpawnDataComponent>().End();
            
            ReactDataPool.BlocksCount.SubOnChange(OnBlockCountChange);
        }

        private void OnBlockCountChange(int value)
        {
            foreach (var entity in _filter)
            {
                ref var delay = ref _world.GetComponent<BlockSpawnDelayComponent>(entity);
                var data = _world.GetComponent<BlockSpawnDataComponent>(entity).Data;
                if (data == null)
                    return;
                
                foreach (var d in data.SpawnData)
                {
                    if (delay.Tier < d.Tier && value >= d.BlocksCount)
                    {
                        delay.Value = d.Delay;
                        delay.Tier = d.Tier;
                        Dbg.LogGreen($"Tier {d.Tier}, block count: {value}, delayTime: {delay.Value}");
                        ReactDataPool.Tier.Value = d.Tier;
                        break;
                    }
                }
                
            }
        }
    }
}