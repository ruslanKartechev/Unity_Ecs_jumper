using Data;
using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;
using UI;
using Zenject;

namespace Ecs.Systems.Init
{
    public class CheckWinConditionSystem : IEcsInitSystem
    {
        [Inject] private IWindowsManager _windowsManager;
        [Inject] private IGlobalSettings _globalSettings;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            ReactDataPool.PlayerHeight.SubOnChange(Check);
        }

        private void Check(float value)
        {
            ref var gameState = ref _world.GetComponent<GameStateComponent>(Pool.PlayerEntity);
            // Dbg.Log($"value: {value}, game state: {gameState.Value}");
            if(value >= _globalSettings.HeightToWin && gameState.Value == EGameState.LevelPlay)
            {
                _windowsManager.CloseBonusWindow();
                _windowsManager.ShowWin();
                gameState.Value = EGameState.Win;
                Pool.World.RemoveComponent<CanMoveComponent>(Pool.PlayerEntity);
                Pool.World.RemoveComponent<CanSpawnComponent>(Pool.PlayerEntity);
            }
        }
    }
}