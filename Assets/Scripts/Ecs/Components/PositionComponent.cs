using UnityEngine;

namespace Ecs.Components
{
    public struct PositionComponent
    {
        public Vector3 Value;

        public PositionComponent(Vector3 value)
        {
            Value = value;
        }
    }
}