using UnityEngine;

namespace Game.Sound.Modifier
{
    public class PitchRandomizer : SoundModifier
    {
        public float Range;
        public override void Apply(PlayingSound sound)
        {
            var mod = UnityEngine.Random.Range(1 - Range, 1 + Range);
            sound.Source.AudSource.pitch *= mod;
        }

        public PitchRandomizer(float range)
        {
            Range = range;
        }

        public static PitchRandomizer PercentRange5 = new PitchRandomizer(5f);
        public static PitchRandomizer PercentRange10 = new PitchRandomizer(10f);
        public static PitchRandomizer PercentRange20 = new PitchRandomizer(20f);

    }
}