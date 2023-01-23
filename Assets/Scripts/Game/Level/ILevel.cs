using System;
namespace  Game.Level
{
    public interface ILevel
    {
        public event Action OnFailed;
        public event Action OnCompleted;
        
        void InitLevel();
        void StartLevel();
        void Win();
        void Fail();
    }
    
}
