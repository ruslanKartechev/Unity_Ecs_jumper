using Data;

namespace Ecs.Components
{
    public struct CellTopsComponent
    {
        public CellTopsData[,] Positions;

        public CellTopsComponent(int length, int width)
        {
            Positions = new CellTopsData[length, width];
        }
    }
}