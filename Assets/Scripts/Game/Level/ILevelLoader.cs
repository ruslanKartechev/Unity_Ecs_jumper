using Game.Level.Impl;

namespace Game.Level
{
    public interface ILevelLoader
    {
        LevelView LoadLevel(int levelIndex);
        void ClearLevel();
        bool EditorMode { get; set; }
    }
}