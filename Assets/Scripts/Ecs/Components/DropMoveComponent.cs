using UnityEngine;

namespace Ecs.Components
{
    public struct DropMoveComponent
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public float Time;
        public float ElapsedTime;
    }
}