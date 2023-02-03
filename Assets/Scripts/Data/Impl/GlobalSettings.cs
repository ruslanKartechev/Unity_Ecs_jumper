using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = nameof(GlobalSettings), menuName = "SO/" + nameof(GlobalSettings))]
    public class GlobalSettings : ScriptableObject, 
        IGlobalSettings
    {
        
        [SerializeField] private float _heightToWin = 100;
        public float HeightToWin => _heightToWin;
    }
}