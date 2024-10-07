using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    private ICharacterStats stats;


    protected override void Awake()
    {
        base.Awake();
        stats = transform.root.GetComponentInChildren<ICharacterStats>();
        if (stats == null)
            Debug.Log("stats ºö");
    }


    private void Die()
    {
        foreach (var particles in deathParticles)
        {
            ParticleManager.StartParticles(particles);
        }
        core.transform.parent.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        stats.OnHealthZero -= Die;
        stats.OnHealthZero += Die;
    }
    private void OnDisable()
    {
        stats.OnHealthZero -= Die; ;
    }
}
