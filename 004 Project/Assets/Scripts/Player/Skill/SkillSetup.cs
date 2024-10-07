using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetup
{
    private Skill currentSkill;
    private SkillDataManager skillDataManager;
    private SkillManager skillManager;
    private GameObject player;
    private GameObject arrow;
    private Transform prefabParent;
    public SkillSetup(GameObject player)
    {
        this.player = player;

        skillDataManager = new SkillDataManager();
        skillManager = new SkillManager();
        InitializePrefab();
        Initialize();
    }

    private void InitializePrefab()
    {
        arrow = GameManager.Resource.Load<GameObject>("Prefabs/Arrow");
        GameObject go = GameObject.Find("SkillPrefab");
        if (go == null)
            go = new GameObject { name = "SkillPrefab" };
        prefabParent = go.transform;
        
        //기타 prefab 초기화
    }
    private void Initialize()
    {
        skillDataManager.Initialize();
        skillManager.LoadSkillData(skillDataManager);
        RegisterPlayerSkills();
        skillManager.InitializeSkill(SkillNames.SpearSkill); // 초기 스킬 설정
    }

    private void RegisterPlayerSkills()
    {
        currentSkill = player.GetComponent<Player>().skill;

        SpearGenerator spearGenerator = new SpearGenerator();
        ExplosionGenerator explosionGenerator = new ExplosionGenerator();

        Vector3 arrowOffset = new Vector3(1.0f, 0, 0); // 화살의 생성 위치 오프셋 설정


        skillManager.RegisterSkill(SkillNames.SpearSkill, currentSkill, new SkillInitializer<SpearSkill, SpearGenerator>(currentSkill, spearGenerator, SkillNames.SpearSkill, arrow, prefabParent, player.transform, arrowOffset));
        skillManager.RegisterSkill(SkillNames.ExplosionSkill, currentSkill, new SkillInitializer<ExplosionSkill, ExplosionGenerator>(currentSkill, explosionGenerator, SkillNames.ExplosionSkill));

        currentSkill.gameObject.name = SkillNames.SpearSkill;
    }

    public Skill GetCurrentSkill()
    {
        return currentSkill;
    }
}
