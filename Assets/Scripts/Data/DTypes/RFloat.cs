using UnityEngine;

namespace Data.DTypes
{
    [System.Serializable]
    public class RFloat
    {
        public float Min;
        public float Max;

        public float Value
        {
            get
            {
                return UnityEngine.Random.Range(Min, Max);
            }    
        }
    }
}