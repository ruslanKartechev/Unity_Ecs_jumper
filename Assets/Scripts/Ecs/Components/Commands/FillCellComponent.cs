using Data;

namespace Ecs.Components
{
    public struct FillCellComponent
    {
        public int x;
        public int y;
        public BlockType Type;

        public FillCellComponent(int x, int y, BlockType type)
        {
            this.x = x;
            this.y = y;
            Type = type;
        }
    }
}