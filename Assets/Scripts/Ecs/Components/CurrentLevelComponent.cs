namespace Ecs.Components
{
    public struct CurrentLevelComponent
    {
        public CurrentLevelComponent(int levelIndex, int totalPassedCount)
        {
            LevelIndex = levelIndex;
            TotalPassedCount = totalPassedCount;
        }

        public int LevelIndex;
        public int TotalPassedCount;
    }
}