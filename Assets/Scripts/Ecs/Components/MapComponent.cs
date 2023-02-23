using System.Collections.Generic;

namespace Ecs.Components
{
    public struct MapComponent
    {
        public int Width;
        public int Height;
        public List<int> AllBlocks;
        public List<int> AllAboveBlocks;
        public float LowestBlockY;
    }
}