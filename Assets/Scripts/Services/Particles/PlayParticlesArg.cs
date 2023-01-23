using UnityEngine;

namespace Services.Particles
{
    public class PlayParticlesArg
    {
        public string name;
        
        public bool timed = true;
        public float duration = 1f;
        
        public Vector3 position;
        public Quaternion rotation;
        
        public Transform parent;

        public PlayParticlesArg Name(string name)
        {
            this.name = name;
            return this;
        }

        public PlayParticlesArg Parent(Transform parent)
        {
            this.parent = parent;
            return this;
        }

        public PlayParticlesArg Duration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public PlayParticlesArg IsTimed(bool timed)
        {
            this.timed = timed;
            return this;
        }

        public PlayParticlesArg Position(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public PlayParticlesArg Rotation(Quaternion rotation)
        {
            this.rotation = rotation;
            return this;
        }
    }
}