using System;
using Game.Sound.Data;
using Game.Sound.Modifier;
using UnityEngine;

namespace Game.Sound
{
    public class PlayingSound : IPlayingSound
    {
        public SoundSource Source;
        public ESoundType Type;
        public float Duration;
        
        public float StartVolume;
        public float StartPitch;
        
        public float ElapsedTime = 0f;
        public Action<PlayingSound> KillCommand { get; set; }
        

        public PlayingSound(SoundSource source)
        {
            Source = source;
            StartVolume = source.AudSource.volume;
            StartPitch = source.AudSource.pitch;
        }
        
        public void Pause()
        {
            Source.AudSource.Stop();
        }

        public void Resume()
        {
            Source.AudSource.Play();
        }

        public void Kill()
        {
            Source.Reset();
            KillCommand?.Invoke(this);
        }

        public void ModifyVolume(float modifier)
        {
            Source.AudSource.volume = StartVolume * modifier;
        }

        public void ModifyPitch(float modifier)
        {
            Source.AudSource.pitch = StartPitch * modifier;
        }
        
        public PlayingSound ApplyModifier(SoundModifier modifier)
        {
            modifier.Apply(this);
            return this;
        }
    }
}