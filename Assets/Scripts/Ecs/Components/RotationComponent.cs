using UnityEngine;

namespace Ecs.Components
{
    public struct RotationComponent
    {
        public Quaternion Value;

        public RotationComponent(Quaternion value)
        {
            Value = value;
        }
    }
}