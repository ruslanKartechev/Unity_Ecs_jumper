using UnityEngine;

namespace Game.Sound.Data
{
    [System.Serializable]
    public class SoundSource
    {
        public string ID;
        public AudioSource AudSource;
        public GameObject Go;
        public Transform DefaultParent;

        public void Reset()
        {
            AudSource.Stop();
            AudSource.clip = null;
            AudSource.spatialBlend = 0f;
            AudSource.maxDistance = 250;
            AudSource.transform.SetParent(DefaultParent);
        }

    }
}