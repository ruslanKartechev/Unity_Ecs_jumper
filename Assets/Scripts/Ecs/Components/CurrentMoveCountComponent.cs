namespace Ecs.Components
{
    public struct CurrentMoveCountComponent
    {
        public int MoveCount;

        public CurrentMoveCountComponent(int moveCount)
        {
            MoveCount = moveCount;
        }
    }
}