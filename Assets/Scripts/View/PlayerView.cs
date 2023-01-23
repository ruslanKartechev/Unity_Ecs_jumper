using Data;
using Data.Impl;
using UnityEngine;

namespace View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings;

        public ParticleSystem poofParticles;
        public IPlayerSettings Settings => _playerSettings;
    }
    
}