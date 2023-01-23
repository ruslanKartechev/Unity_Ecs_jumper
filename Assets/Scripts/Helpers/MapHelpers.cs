using Data;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using UnityEngine;

namespace Helpers
{
    public class MapHelpers
    {
        public static CellTopsData GetTopsDataAtCell(int x, int y)
        {
            var world = Pool.World;
            ref var tops = ref world.GetComponent<CellTopsComponent>(Pool.MapEntity);
            return tops.Positions[x, y];   
        }
        
        public static Vector3 GetPositionAtCell(int x, int y)
        {
            var world = Pool.World;
            ref var tops = ref world.GetComponent<CellTopsComponent>(Pool.MapEntity);
            return tops.Positions[x, y].Position;
        }
    }
}