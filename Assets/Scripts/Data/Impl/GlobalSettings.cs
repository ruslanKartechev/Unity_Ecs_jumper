using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = nameof(GlobalSettings), menuName = "SO/" + nameof(GlobalSettings))]
    public class GlobalSettings : ScriptableObject, 
        IGlobalSettings
    {
        [SerializeField] private LayerMask _floorMask;
        
        [Header("DriftAreaSettings")]
        [SerializeField] private float _driftAreaStartminSpeed = 10;
        [SerializeField] private float _exitTime;
        [SerializeField] private float _exitSpeedMod;
        [SerializeField] private float _maxOffset;
        [SerializeField] private float _driftDeceleration;
        [SerializeField] private float _driftRotSpeed;
        [Header("Respawn")]
        [SerializeField] private float _percentDistanceToCrash;
        [SerializeField] private float _repawnPercentDistance;
        [SerializeField] private float _respawnDelay;
        [SerializeField] private float _respawnVerticalOffset;
        
        [Header("Recenter")]
        [SerializeField] private float _speedStuckThreshold = 1.5f;
        [SerializeField] private float _stuckTimeMax = 0.2f;
        [SerializeField] private float _spawnUpOffset = 3f;
        [Header("Movement")] 
        [SerializeField] private float _breakApplySpeedDiff = 2;
        [SerializeField] private float _randomizedOffset = 0.5f;
        
        public LayerMask FloorMask => _floorMask;

        public float DriftExitTime => _exitTime;
        public float DriftMaxOffset => _maxOffset;
        public float DriftExitSpeedMod => _exitSpeedMod;
        public float DriftDeceleration => _driftDeceleration;
        public float DriftRotationSpeed => _driftRotSpeed;
        public float DriftAreaStartMinSpeed => _driftAreaStartminSpeed;


        public float PercentDistanceToCrash => _percentDistanceToCrash / 100;
        public float RespawnPercentDistance => _repawnPercentDistance / 100f;
        public float RespawnDelay => _respawnDelay;
        public float RespawnVerticalOffset => _respawnVerticalOffset;


        public float SpeedStuckThreshold => _speedStuckThreshold;
        public float StuckTimeMax => _stuckTimeMax;
        public float SpawnUpOffset => _spawnUpOffset;
        public float BreakApplySpeedDifference => _breakApplySpeedDiff;
        
        public float RandomizedOffset => _randomizedOffset;

    }
}