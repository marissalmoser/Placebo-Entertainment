/*****************************************************************************
// File Name :         WaterBehavior.cs
// Author :            Mark 
// Contributors:       Andrea Swihart-DeCoster
// Creation Date :     6/19/2024
//
// Brief Description : Controls the water particle triggers on the fire.
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    private ParticleSystem _particleSyst;
    private GameObject _currentFireObject;

    private void Start()
    {
        _particleSyst = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        // Particles inside and particles that have exited the collider
        List<ParticleSystem.Particle> particlesInside = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> particlesExited = new List<ParticleSystem.Particle>();

        // Contains the particle collision info e.g the collider the particle collided with
        ParticleSystem.ColliderData insideColliderData;

        // get the particles which matched the trigger conditions this frame
        int numParticleInside = _particleSyst.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particlesInside, out insideColliderData);
        int numParticlesExited = _particleSyst.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, particlesExited);

        // If there are no particles in the fire collider, start fire growth
        if (numParticlesExited > 0 && numParticleInside <= 0)
        {
            if (_currentFireObject.TryGetComponent<FireBehavior>(out FireBehavior _fireBehavior))
            {
                _fireBehavior.StartFireGrow();
            }
        }
        // If there are particles in the fire collider...
        else if (particlesInside.Count > 0)
        {
            // Make sure the collider count is valid.
            // If the fire is destroyed before this is called OR if the particles all exit before this is called, it will throw a null ref
            if (insideColliderData.GetColliderCount(0) > 0)
            {
                if (!_currentFireObject)
                {
                    _currentFireObject = insideColliderData.GetCollider(0, 0).gameObject;
                }
            }

            if (_currentFireObject.TryGetComponent<FireBehavior>(out FireBehavior _fireBehavior))
            {
                _fireBehavior.StartFireShrink();
            }
        }   
    }
}
