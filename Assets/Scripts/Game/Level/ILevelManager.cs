namespace Game.Level
{
    public interface ILevelManager
    {
        void LoadCurrent();
        void NextLevel();
        void PreviousLevel();
        int TotalLevelsCompleted { get; set; }
    }
}