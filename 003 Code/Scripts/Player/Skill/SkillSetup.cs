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
        arrow = GameManager.Resource.Load<GameObject>("Prefabs/Ranges/Arrow");
        rock = GameManager.Resource.Load<GameObject>("Prefabs/Rock");
        GameObject go = GameObject.Find("SkillPrefab");
        if (go == null)
            go = new GameObject { name = "SkillPrefab" };
        prefabParent = go.transform;
        
        //기타 prefab 초기화
    }
    private void Initialize(string skillname)
    {
        skillDataManager.Initialize();
        skillManager.LoadSkillData(skillDataManager);
        RegisterPlayerSkills();
        skillManager.InitializeSkill(skillname); // 초기 스킬 설정
        currentSkill.gameObject.name = skillname; // 이름 변경
    }

    private void RegisterPlayerSkills()
    {
        currentSkill = player.GetComponent<Player>().skill;

        FireGenerator fireGenerator = new FireGenerator();
        IceGenerator iceGenerator = new IceGenerator();
        LandGenerator landGenerator = new LandGenerator();
        LightGenerator lightGenerator = new LightGenerator();

        Vector3 arrowOffset = new Vector3(1.0f, 0, 0); // 화살의 생성 위치 오프셋 설정
        Vector3 rockOffset = new Vector3(4.0f, 0.82f, 0); // 화살의 생성 위치 오프셋 설정


        skillManager.RegisterSkill(SkillNames.FireSkill, currentSkill, new SkillInitializer<FireSkill, FireGenerator>(currentSkill, fireGenerator, SkillNames.FireSkill));
        skillManager.RegisterSkill(SkillNames.IceSkill, currentSkill, new SkillInitializer<IceSkill, IceGenerator>(currentSkill, iceGenerator, SkillNames.IceSkill));
        skillManager.RegisterSkill(SkillNames.LandSkill, currentSkill, new SkillInitializer<LandSkill, LandGenerator>(currentSkill, landGenerator, SkillNames.FireSkill, rock, prefabParent, player.transform, rockOffset));
        skillManager.RegisterSkill(SkillNames.LightSkill, currentSkill, new SkillInitializer<LightSkill, LightGenerator>(currentSkill, lightGenerator, SkillNames.LightSkill, arrow, prefabParent, player.transform, arrowOffset));
    }

    public Skill GetCurrentSkill()
    {
        return currentSkill;
    }

    public void ChangeSkill(Element newElement)
    {
        string newSkill = SkillNames.NoneSkill;
        switch (newElement)
        {
            case Element.None:
                newSkill = SkillNames.NoneSkill;
                break;
            case Element.Fire:
                newSkill = SkillNames.FireSkill;
                break;
            case Element.Ice:
                newSkill = SkillNames.IceSkill;
                break;
            case Element.Land:
                newSkill = SkillNames.LandSkill;
                break;
            case Element.Light:
                newSkill = SkillNames.LightSkill;
                break;
            default:
                newSkill = SkillNames.NoneSkill;
                break;

        }
        skillManager.ChangeSkill(newSkill);
    }
}
