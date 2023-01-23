using UnityEngine;

namespace Ecs.Components
{
    public struct LerpMoveComponent
    {
        public LerpMoveComponent(Vector3 startPosition, Vector3 endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Value = 0f;
        }

        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public float Value;

    }
}