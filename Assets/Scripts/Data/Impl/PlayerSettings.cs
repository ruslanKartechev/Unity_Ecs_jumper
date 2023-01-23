using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "SO/PlayerSettings")]
    public class PlayerSettings : ScriptableObject, IPlayerSettings
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _upOffset;
        [SerializeField] private float _upOffsetLerpValue;
        [SerializeField] private float _maxJumpHeight;

        
        public float MoveSpeed => _moveSpeed;
        public float UpOffset => _upOffset;
        public float UpOffsetLerpValue => _upOffsetLerpValue;
        public float MaxJumpHeight => _maxJumpHeight;
    }
}