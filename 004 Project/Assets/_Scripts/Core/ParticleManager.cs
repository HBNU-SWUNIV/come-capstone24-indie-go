using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : CoreComponent
{
  //  private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();

       // particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation, Transform defender, float time = 0)
    {
        GameObject go = Instantiate(particlePrefab, position, rotation, defender);
        if (time != 0)
        {
           // Destroy(go, time);
        }
        return go;
    }

    public GameObject StartParticles(GameObject particlePrefab, Transform defender)
    {
        return StartParticles(particlePrefab, transform.position, Quaternion.identity, defender.Find("Particles"));
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab, Transform defender)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        return StartParticles(particlePrefab, transform.position, randomRotation, defender.Find("Particles"));
    }

    public GameObject StartParticlesWithDesignatedRotation(GameObject particlePrefab, Vector3 designateRotation, Transform defender)
    {
        return StartParticles(particlePrefab, transform.position + designateRotation, Quaternion.identity, defender);
    }

    public GameObject StartParticlesWithDesignatedRotationAndDestroy(GameObject particlePrefab, Vector3 designateRotation, Transform defender, float time)
    {
        return StartParticles(particlePrefab, transform.position + designateRotation, Quaternion.identity, defender, time);

    }
}
