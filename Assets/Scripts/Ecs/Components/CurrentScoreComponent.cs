namespace Ecs.Components
{
    public struct CurrentScoreComponent
    {
        public int Score;

        public CurrentScoreComponent(int score)
        {
            Score = score;
        }
    }
}