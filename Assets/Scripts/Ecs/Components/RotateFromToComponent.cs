using UnityEngine;

namespace Ecs.Components
{
    public struct RotateFromToComponent
    {
        public Vector3 From;
        public Vector3 To;
        public float Time;
        public float Elapsed;
    }
}