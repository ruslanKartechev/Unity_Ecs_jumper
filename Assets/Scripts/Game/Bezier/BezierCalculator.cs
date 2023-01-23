using UnityEngine;

namespace Game.Bezier
{
    public class BezierCalculator
    {
        public static Vector3 CalculatePosition(Vector3 start, Vector3 inflection, Vector3 end, float t)
        {
            return Mathf.Pow((1 - t), 2) * start + 2 * (1 - t) * t * inflection + Mathf.Pow(t, 2) * end;
        }

        public static Vector3[] CalculatePositions(Vector3 start, Vector3 inflection, Vector3 end, int count)
        {
            var positions = new Vector3[count];
            for (var i = 0; i < count; i++)
            {
                var t = (float) i / (count - 1);
                var p = Mathf.Pow((1 - t), 2) * start + 2 * (1 - t) * t * inflection + Mathf.Pow(t, 2) * end;
                positions[i] = p;
            }
            return positions;
        }
    }
}