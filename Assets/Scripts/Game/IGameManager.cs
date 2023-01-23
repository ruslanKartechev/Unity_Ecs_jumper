namespace Game
{
    public interface IGameManager
    {
        public void OnStart();

        public void OnQuit();
        
        void StartLevelPlay();
        void WinLevel();
        void Fail();
        void NextLevel();
        void Replay();
    }
}