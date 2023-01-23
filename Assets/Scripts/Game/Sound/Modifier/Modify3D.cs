using UnityEngine;

namespace Game.Sound.Modifier
{
    public class Modify3D : SoundModifier
    {
        public float Blend;
        public AudioRolloffMode RolloffMode;
        public float MaxDistance;

        public Modify3D(float blend, AudioRolloffMode mode, float maxDistance)
        {
            Blend = blend;
            RolloffMode = mode;
            MaxDistance = maxDistance;
        }

        public static Modify3D DefaultLinear = new Modify3D(1f, AudioRolloffMode.Linear, 25f);
        public static Modify3D DefaultLinearFar = new Modify3D(1f, AudioRolloffMode.Linear, 50f);

        public override void Apply(PlayingSound playingSound)
        {
            var source = playingSound.Source.AudSource;
            source.rolloffMode = RolloffMode;
            source.spatialBlend = Blend;
            source.maxDistance = MaxDistance;
        }
    }
}