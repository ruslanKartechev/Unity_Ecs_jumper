using Game.Level.Impl;

namespace Data
{
    public interface ILevelRepository
    {
        LevelView GetLevel(int index);
        int MaxIndex { get; }
    }
}