using UnityEngine;

namespace Game.Sound.Data
{
    public class SoundPlayArgs
    {
        public string name;
        
        /// <summary>
        /// By default, the sound is played once
        /// </summary>
        public bool once = true;
        
        /// <summary>
        /// if Loop is on and Duration == -1, the sound is played forever
        /// </summary>
        public bool loop = false;
        
        /// <summary>
        /// If duration > 0, this custom duration is used in loop
        /// </summary>
        public float duration = -1f;


        /// <summary>
        /// if position is not Vector3.negativeInfinity, the position is used
        /// </summary>
        public bool applyPosition = true;
        public Vector3 position;
        
        /// <summary>
        /// if parent != null, the sound source is parented there
        /// </summary>
        public Transform parent;

        public bool autoPlay = true;


        public SoundPlayArgs Name(string name)
        {
            this.name = name;
            return this;
        }

        public SoundPlayArgs Loop(bool loop)
        {
            this.loop = loop;
            return this;
        }

        public SoundPlayArgs ApplyPosition(bool apply)
        {
            this.applyPosition = apply;
            return this;
        }

        public SoundPlayArgs Position(Vector3 pos)
        {
            position = pos;
            return this;
        }

        public SoundPlayArgs Duration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public SoundPlayArgs Parent(Transform parent)
        {
            this.parent = parent;
            return this;
        }

        public SoundPlayArgs AutoPlay(bool play)
        {
            this.autoPlay = play;
            return this;
        }
        
    }
}