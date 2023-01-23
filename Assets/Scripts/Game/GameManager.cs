using Game.Level;
using UI;
using Zenject;

namespace Game
{
    // needed for debugging
    public class GameManager : IGameManager
    {
            // Unity Message
            [Inject] private ILevelManager _levelManager;
            [Inject] private IWindowsManager _uiManager;
            
            public void OnStart() {}

            public void OnQuit() {}

            public void StartLevelPlay()
            {

            }

            public void WinLevel()
            {
      
            }

            public void Fail()
            {
       
            }

            public void NextLevel()
            {
                // CommandsPool.ExecuteCommand(new ClearLevelCommand());
                _uiManager.ShowStart();
                _levelManager.NextLevel();
     
            }

            public void Replay()
            {

                _levelManager.LoadCurrent();
                _uiManager.ShowStart();
            }

            


    }
}