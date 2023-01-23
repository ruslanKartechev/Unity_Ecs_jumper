using UnityEngine;

namespace Services.Particles.Service
{
    public interface IParticlesService
    {
        ParticleSystem PlayParticles(PlayParticlesArg args);
    }
}