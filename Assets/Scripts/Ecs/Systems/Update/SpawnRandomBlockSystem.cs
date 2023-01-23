using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Data;
using Data.DTypes;
using Ecs.Components;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class SpawnRandomBlockSystem : IEcsInitSystem, IEcsRunSystem
    {
        private CellWeightedPicker _weightedPicker;
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            _weightedPicker = new CellWeightedPicker();
            _filter = systems.GetWorld().Filter<SpawnRandomBlockComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var world = systems.GetWorld();
                world.RemoveComponent<SpawnRandomBlockComponent>(entity);
                ref var playerCellPos = ref world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
                ref var cellTops = ref world.GetComponent<CellTopsComponent>(Pool.MapEntity);
                
                var list = MakeOrderedList(ref cellTops, ref playerCellPos);
                _weightedPicker.CreateOptions(list, playerCellPos);
                
                var result = _weightedPicker.GetOption();
                
                ref var command = ref world.AddComponentToEntity<FillCellComponent>(Pool.MapEntity);
                command.x = result.x;
                command.y = result.y;
                command.Type = BlockType.Default;
            }
        }
        
        private List<CellPickData> MakeOrderedList(ref CellTopsComponent cellTops, ref CellPositionComponent playerPosition)
        {
            var list = new List<CellPickData>();
            var listIndex = 0;
            for (int i = 0; i < cellTops.Positions.GetLength(0); i++)
            {
                for (int k = 0; k < cellTops.Positions.GetLength(1); k++)
                {
                    var pickData = new CellPickData();
                    pickData.height = cellTops.Positions[i, k].Position.y;
                    pickData.x = i;
                    pickData.y = k;
                    list.Add(pickData);
                    listIndex++;
                }
            }
            list.Sort((a, b) => a.height > b.height ? 1 : -1);
            return list;
        }

        private int GetRandomX(int forbidden, int max)
        {
            var result = forbidden;
            do
            {
                result = UnityEngine.Random.Range(0, max);
            } while (forbidden == result);

            return result;
        }
        
        
        



        public class CellPickData
        {
            public int x;
            public int y;
            public float height;
        }

        public class CellWeightedPicker : TWeightedPicker<CellPickData>
        {
            public CellWeightedPicker()
            {
                Options = new List<TWeighted<CellPickData>>();
            }
            
            public void CreateOptions(List<CellPickData> data, CellPositionComponent playerComponent)
            {
                _total = 0;
                Options.Clear();
                var heighest = data[data.Count - 1];
                var i = 0;
                foreach (var tw in data)
                {
                    var pickData = new TWeighted<CellPickData>();
                    pickData.Value = tw;

                    if (tw.x == playerComponent.x && tw.y == playerComponent.y)
                    {
                        pickData.Weight = -1;
                    }
                    else
                    {
                        pickData.Weight = heighest.height - tw.height + 1;
                        _total += pickData.Weight;
                    }
                    Options.Add(pickData);
                    i++;
                }
            }
        }

        
    }
}