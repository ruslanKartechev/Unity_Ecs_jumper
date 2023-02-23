using System.Collections.Generic;
using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "TearBlockColorData", menuName = "SO/TearBlockColorData")]
    public class TearBlockMatData : ScriptableObject, ITearBlockMatData
    {
        [SerializeField] private List<Material> _colors;
        
        public Material GetMaterial(int tear)
        {
            if (tear >= _colors.Count)
                return _colors[_colors.Count - 1];
            return _colors[tear];
        }
    }
}