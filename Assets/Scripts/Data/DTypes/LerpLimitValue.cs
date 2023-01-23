using System;
using UnityEngine;

namespace Data.DTypes
{
    [System.Serializable]
    public class LerpLimitValue
    {
        [Header("Values")]
        public float A_value;
        public float B_value;
        [Header("Limits")]
        public float A_limit;
        public float B_limit;
        [SerializeField] private bool _dropFromA;
        [SerializeField] private float _aDropValue;

        public float GetValue(float t)
        {
            if (_dropFromA)
            {
                if (t < A_limit)
                {
                    return _aDropValue;
                }
            }
            var inverse = Mathf.InverseLerp(A_limit, B_limit, t);
            var val = Mathf.Lerp(A_value, B_value, inverse);
            return val;
        }
    }
}