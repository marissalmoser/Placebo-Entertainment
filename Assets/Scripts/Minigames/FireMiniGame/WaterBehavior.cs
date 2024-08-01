/*****************************************************************************
// File Name :         WaterBehavior.cs
// Author :            Mark Hanson
// Creation Date :     6/19/2024
//
// Brief Description : Function for destroying water when its in contact with specific things.
*****************************************************************************/
using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    [SerializeField] ParticleSystem _ps;
    /* [SerializeField] private Rigidbody _rb;
     [SerializeField] private GameObject _cam;

     void Awake()
     {
         _cam = GameObject.FindWithTag("MainCamera");
         _rb.AddForce(_cam.transform.forward * 12f, ForceMode.Impulse);
     }

     void OnTriggerEnter(Collider col)
     {
         if (col.gameObject.name != "FishHose" && col.gameObject.tag != "Water")
         {
             Destroy(gameObject);
         }
     }*/
    private void OnParticleTrigger()
    {
        print(_ps.trigger.GetCollider(0).gameObject.name);

        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exitedParticles = new List<ParticleSystem.Particle>();

        ParticleSystem.ColliderData enteredColliderData;
        ParticleSystem.ColliderData exitedColliderData;

        // get the particles which matched the trigger conditions this frame
        int numParticlesEntered = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, enteredParticles, out enteredColliderData);
        int numParticlesExited = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitedParticles, out exitedColliderData);
        
        foreach (ParticleSystem.Particle particle in exitedParticles)
        {
            // Loop through colliders in exitedColliderData
            // Loop through enteredParticles
                // Check if the particle contains this collider
                    // if not, call the WaterCooldown Function()
        }

        foreach (ParticleSystem.Particle particle in enteredParticles)
        {
            // Loop through colliders in enteredColliderData
                // Start to shrink the fire
        }
    }
}
