using System.Collections.Generic;
using Helpers;
using Services.Parent;
using UnityEngine;
using Zenject;

namespace Services.Particles.Service.Impl
{
    public class ParticlesService : MonoBehaviour, IParticlesService
    {
        [Inject] private IParentService _parentService;
        [Inject] private IParticlesRepository _particlesRepository;
        private Dictionary<string, HashSet<TimedData>> _timedData = new Dictionary<string, HashSet<TimedData>>();

        public class TimedData
        {
            public string Name;
            public bool IsAvailable;
            public ParticleSystem ParticleSystem;
            public float Elapsed;
            public float Duration;
        }
        
        public ParticleSystem PlayParticles(PlayParticlesArg args)
        {
            var particles = GetParticles(args.name);
            // Dbg.LogRed($"Playing particles: {args.name}");
            if (args.parent != null)
            {
                particles.transform.parent = args.parent;
            }
            else
            {
                particles.transform.parent = _parentService.DefaultParent;
            }
            particles.gameObject.SetActive(true);
            particles.transform.position = args.position;
            particles.transform.rotation = args.rotation;

            if (args.timed)
            {
                var timedData = new TimedData()
                {
                    Name = args.name, Duration = args.duration, Elapsed = 0f, IsAvailable = false,
                    ParticleSystem = particles
                };
                if (_timedData.ContainsKey(args.name))
                {
                    _timedData[args.name].Add(timedData);
                }
                else
                {
                    _timedData.Add(args.name, new HashSet<TimedData>(){timedData});
                }
            }
            particles.Play();
            return particles;
        }

        
        private ParticleSystem GetParticles(string particleName)
        {
            if (_timedData.ContainsKey(particleName) == false)
            {
                return GetFromRepository(particleName);
            }

            var dataSet = _timedData[particleName];
            foreach (var data in dataSet)
            {
                if (data.IsAvailable && data.ParticleSystem != null)
                {
                    return data.ParticleSystem;
                }
            }
                
            return GetFromRepository(particleName);
        }
        
        private ParticleSystem GetFromRepository(string name)
        {
            // Dbg.Log("getting particles from repository");
            var particlesPrefab = _particlesRepository.GetPrefab(name);
            var instance = Instantiate(particlesPrefab);
            return instance;
        }
        
        private void Update()
        {
            var keys = _timedData.Keys;
            foreach (var key in keys)
            {
                var dataSet = _timedData[key];
                foreach (var data in dataSet)
                {
                    if(data.IsAvailable)
                        continue;
                    if (data.ParticleSystem == null)
                    {
                        data.IsAvailable = false;
                        continue;
                    }
                    
                    data.Elapsed += UnityEngine.Time.deltaTime;
                    if (data.Elapsed > data.Duration)
                    {
                        data.IsAvailable = true;
                        data.Elapsed = 0f;
                        data.ParticleSystem.Stop();
                        data.ParticleSystem.transform.position = Vector3.zero;
                        // Dbg.Log("returned particles to pool");
                    }
                }
                dataSet.RemoveWhere(t => t == null);
            }
        }
    }
    
    
    
}