using UnityEngine;

namespace Data
{
    public interface IGlobalSettings
    {
        LayerMask FloorMask { get; }
        
        // Drifting
        float DriftExitTime { get; }
        float DriftMaxOffset { get; }
        float DriftExitSpeedMod { get; }
        float DriftDeceleration { get; }
        float DriftRotationSpeed { get; }
        float DriftAreaStartMinSpeed { get; }


        float PercentDistanceToCrash { get; }
        float RespawnPercentDistance { get; }
        float RespawnDelay { get; }
        float RespawnVerticalOffset { get; }
        
        float RandomizedOffset { get; }
        
        
        float SpeedStuckThreshold { get; }
        float StuckTimeMax { get; }
        float SpawnUpOffset { get; }
        
        float BreakApplySpeedDifference { get; }

        
    }
}