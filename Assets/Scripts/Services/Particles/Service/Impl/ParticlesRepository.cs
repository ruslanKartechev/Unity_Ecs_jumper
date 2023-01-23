using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Services.Particles.Service.Impl
{
    [CreateAssetMenu(fileName = nameof(ParticlesRepository), menuName = "SO/" + nameof(ParticlesRepository))]
    public class ParticlesRepository : ScriptableObject, IParticlesRepository
    {
        [SerializeField] private List<Data> _data = new List<Data>();
        private Dictionary<string, ParticleSystem> _prefabsByName = new Dictionary<string, ParticleSystem>();

        [System.Serializable]
        public class Data
        {
            public string Name;
            public ParticleSystem ParticlePrefab;
        }

        private void OnEnable()
        {
            foreach (var data in _data)
            {
                if(data == null)
                    continue;
                _prefabsByName.Add(data.Name, data.ParticlePrefab);
            }   
        }


        public ParticleSystem GetPrefab(string name)
        {
            if (_prefabsByName.ContainsKey(name) == false)
            {
                Dbg.LogRed($"ParticlesRepo has no: {name} prefab");
                return null;
            }
            return _prefabsByName[name];
        }
    }
}