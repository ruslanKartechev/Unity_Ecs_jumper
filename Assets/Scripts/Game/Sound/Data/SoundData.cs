using UnityEngine;

namespace Game.Sound.Data
{
    [System.Serializable]
    public class SoundData
    {
        public AudioClip Clip;
        [Range(0f,1f)] public float Volume;
        [Range(0f,1f)] public float Pitch;
    }
}