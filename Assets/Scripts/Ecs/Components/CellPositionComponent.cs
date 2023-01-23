namespace Ecs.Components
{
    public struct CellPositionComponent
    {
        public CellPositionComponent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
        
    }
}