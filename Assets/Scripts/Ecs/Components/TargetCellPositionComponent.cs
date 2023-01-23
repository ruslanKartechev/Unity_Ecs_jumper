namespace Ecs.Components
{
    public struct TargetCellPositionComponent
    {
        public TargetCellPositionComponent(int y, int x)
        {
            this.y = y;
            this.x = x;
        }

        public int y;
        public int x;
    }
}