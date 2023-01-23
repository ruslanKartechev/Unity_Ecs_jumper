using System.Collections.Generic;
using Game.Sound.Data;
using Game.Sound.Modifier;
using Helpers;
using UnityEngine;
using Zenject;

namespace Game.Sound.Impl
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        [Inject] private ISoundRepository _soundRepository;
        [Inject] private IAudioSourceProvider _audioSourceProvider;
        [Inject] private IGlobalSoundSettings _globalSoundSettings;

        private bool _inited;
        
        private HashSet<PlayingSound> _playingSounds = new HashSet<PlayingSound>();
        private HashSet<PlayingSound> _loopedSounds = new HashSet<PlayingSound>();

        private HashSet<PlayingSound> _killSound = new HashSet<PlayingSound>();

        public void Init()
        {
            if (_inited )
                return;
            _inited = true;
            _soundRepository.Init();
            _audioSourceProvider.Init();
        }

        public PlayingSound PlaySound(SoundPlayArgs args)
        {
            var sound = _soundRepository.GetSound(args.name);
            var source = _audioSourceProvider.GetFreeAudioSource();
            if(sound == null)
                Dbg.LogRed($"sound: {args.name} is null");
            
            if(source == null)
                Dbg.LogRed($"source for: {args.name} is null");

            if (sound == null || source == null)
                return null;
            var playingSound = new PlayingSound(source);
            source.AudSource.clip = sound.Clip;
            playingSound.KillCommand = KillSound;
            source.AudSource.loop = false;
            playingSound.Type = sound.Type;
            if (args.once)
            {
                playingSound.Duration = sound.Clip.length;
            } 
            else if (args.duration > 0)
            {
                playingSound.Duration = args.duration;
                source.AudSource.loop = true;
            }

            if (args.loop)
            {
                _loopedSounds.Add(playingSound);
                source.AudSource.loop = true;
            }
            else
            {
                _playingSounds.Add(playingSound);
            }

            if (args.applyPosition)
            {
                source.Go.transform.position = args.position;
            }

            if (args.parent != null)
            {
                source.Go.transform.parent = args.parent;
            }

            var volume = sound.BaseVolume;
            if (sound.Type == ESoundType.Default)
            {
                volume *= _globalSoundSettings.MasterVolume;
                source.AudSource.mute = !_globalSoundSettings.SoundEnabled;
            }
            else if (sound.Type == ESoundType.Music)
            {
                volume *= _globalSoundSettings.MusicVolume;
                source.AudSource.mute = !_globalSoundSettings.MusicEnabled;
            }

            var pitch = sound.BasePitch;
            source.AudSource.volume = volume;
            source.AudSource.pitch = pitch;
            
            playingSound.StartVolume = volume;
            playingSound.StartPitch = pitch;
            if(args.autoPlay)
                source.AudSource.Play();
            return playingSound;
        }

        public void AllSoundEnabled(bool active)
        {
            foreach (var sound in _playingSounds)
            {
                if(sound.Type == ESoundType.Default)
                    sound.Source.AudSource.mute = !active;
            }   
        }

        public void AllMusicEnabled(bool active)
        {
            foreach (var sound in _playingSounds)
            {
                if(sound.Type == ESoundType.Music)
                    sound.Source.AudSource.mute = !active;
            }   
        }
        
        private void Update()
        {
            foreach (var sound in _playingSounds)
            {
                // if (sound.Source.AudSource == null)
                // {
                // }
                sound.ElapsedTime += Time.deltaTime;
                if (sound.ElapsedTime >= sound.Duration)
                {
                    sound.KillCommand?.Invoke(sound);
                }
            }

            foreach (var sound in _killSound)
            {
                sound.Source.AudSource.Stop();
                _playingSounds.Remove(sound);
                _loopedSounds.Remove(sound);
                _audioSourceProvider.ReturnSource(sound.Source);
            }
            _killSound.Clear();
        }

        private void KillSound(PlayingSound playingSound)
        {
            _killSound.Add(playingSound);
        }

        public void ApplyModifiers(PlayingSound playingSound, List<SoundModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                modifier.Apply(playingSound);
            }
        }

        public void ApplyModifier(PlayingSound playingSound, SoundModifier modifier)
        {
            modifier.Apply(playingSound);
        }

        public void ApplyTimeScale(float timeScale)
        {
            // Dbg.LogRed($"modifying pitch {timeScale}");
            foreach (var sound in _playingSounds)
            {
                sound.ModifyPitch(timeScale);
            }
        }
    }
}