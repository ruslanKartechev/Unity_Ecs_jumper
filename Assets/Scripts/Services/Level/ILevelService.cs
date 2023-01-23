using Game.Level;
using UnityEngine;

namespace Services.Level
{
    public interface ILevelService
    {
        int CurrentLevelIndex { get; set; }
        int TotalLevelsPassed { get; set; }
        int NextLevel();
        int PrevLevel();
        int CorrectIndex(int index);
        GameObject GetPrefab(int index);
        int GetTotalCount();
    }
}