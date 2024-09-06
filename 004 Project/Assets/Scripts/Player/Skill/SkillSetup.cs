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
    private GameObject rock;
    private Transform prefabParent;
    private string currentSkillname;
    public SkillSetup(GameObject player)
    {
        this.player = player;
        currentSkillname = SkillNames.IceSkill;
        skillDataManager = new SkillDataManager();
        skillManager = new SkillManager();
        InitializePrefab();
        Initialize(currentSkillname);
    }

    private void InitializePrefab()
    {
        arrow = GameManager.Resource.Load<GameObject>("Prefabs/Arrow");
        rock = GameManager.Resource.Load<GameObject>("Prefabs/Rock");
        GameObject go = GameObject.Find("SkillPrefab");
        if (go == null)
            go = new GameObject { name = "SkillPrefab" };
        prefabParent = go.transform;
        
        //��Ÿ prefab �ʱ�ȭ
    }
    private void Initialize(string skillname)
    {
        skillDataManager.Initialize();
        skillManager.LoadSkillData(skillDataManager);
        RegisterPlayerSkills();
        skillManager.InitializeSkill(skillname); // �ʱ� ��ų ����
        currentSkill.gameObject.name = skillname; // �̸� ����
    }

    private void RegisterPlayerSkills()
    {
        currentSkill = player.GetComponent<Player>().skill;

        SpearGenerator spearGenerator = new SpearGenerator();
        FireGenerator fireGenerator = new FireGenerator();
        IceGenerator iceGenerator = new IceGenerator();
        LandGenerator landGenerator = new LandGenerator();

        Vector3 arrowOffset = new Vector3(1.0f, 0, 0); // ȭ���� ���� ��ġ ������ ����
        Vector3 rockOffset = new Vector3(4.0f, 0.82f, 0); // ȭ���� ���� ��ġ ������ ����


        skillManager.RegisterSkill(SkillNames.SpearSkill, currentSkill, new SkillInitializer<SpearSkill, SpearGenerator>(currentSkill, spearGenerator, SkillNames.SpearSkill, arrow, prefabParent, player.transform, arrowOffset));
        skillManager.RegisterSkill(SkillNames.FireSkill, currentSkill, new SkillInitializer<FireSkill, FireGenerator>(currentSkill, fireGenerator, SkillNames.FireSkill));
        skillManager.RegisterSkill(SkillNames.IceSkill, currentSkill, new SkillInitializer<IceSkill, IceGenerator>(currentSkill, iceGenerator, SkillNames.IceSkill));
        skillManager.RegisterSkill(SkillNames.LandSkill, currentSkill, new SkillInitializer<LandSkill, LandGenerator>(currentSkill, landGenerator, SkillNames.FireSkill, rock, prefabParent, player.transform, rockOffset));
        
    }

    public Skill GetCurrentSkill()
    {
        return currentSkill;
    }
}
