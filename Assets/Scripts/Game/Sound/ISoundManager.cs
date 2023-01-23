using System.Collections.Generic;
using Game.Sound.Data;
using Game.Sound.Modifier;

namespace Game.Sound
{
    public interface ISoundManager
    {
        void Init();
        PlayingSound PlaySound(SoundPlayArgs args);
        void ApplyModifiers(PlayingSound playingSound, List<SoundModifier> modifiers);
        void ApplyModifier(PlayingSound playingSound, SoundModifier modifier);
        void ApplyTimeScale(float timeScale);

    }
}