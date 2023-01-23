using UnityEngine;

namespace Game.Sound.Data
{
    [System.Serializable]
    public class SoundStoreData
    {
        public string Name;
        public AudioClip Clip;
        public ESoundType Type;
        [Range(0f,1f)]  public float BaseVolume;
        [Range(0f,1f)] public float BasePitch;
    }
}