namespace Data
{
    public interface IPlayerSettings
    {
        float MoveSpeed { get; }
        float UpOffset { get; }
        float UpOffsetLerpValue { get; }
        float MaxJumpHeight { get; }
    }
}