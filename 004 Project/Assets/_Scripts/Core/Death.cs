using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;
    private Transform deathParticleParent;
    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    private ICharacterStats stats;


    protected override void Awake()
    {
        base.Awake();
        stats = transform.root.GetComponentInChildren<ICharacterStats>();
        if (stats == null)
            Debug.Log("stats ��");
        GameObject go = GameObject.Find("DeathParticles");
        if (go == null)
        {
            go = new GameObject() { name = "DeathParticles" };
        }
        deathParticleParent = go.transform;
    }

    private void Die()
    {

        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("GameOverManager를 찾을 수 없습니다.");
            }
        }

        foreach (var particles in deathParticles)
        {
            ParticleManager.StartParticles(particles, deathParticleParent);
        }

        transform.root.GetComponentInChildren<ICharacterStats>().isDead = true;
        core.transform.parent.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        stats.OnHealthZero -= Die;
        stats.OnHealthZero += Die;
    }
    private void OnDisable()
    {
        stats.OnHealthZero -= Die;

    }
}
