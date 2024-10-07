using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private Dictionary<string, SkillDataEx> skillDataDict = new Dictionary<string, SkillDataEx>();
    private Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
    private Dictionary<string, SkillInitializer> initializers = new Dictionary<string, SkillInitializer>();
    //스킬을 변경한다면 현재 스킬을 받아오기 위해 사용
    private string currentSkillName;
    private Skill currentSkill;

    // 스킬 데이터를 미리 로드
    public void LoadSkillData(SkillDataManager skillDataManager)
    {
        foreach (var skillName in skillDataManager.GetAllSkillNames())
        {
            var data = skillDataManager.GetSkillData(skillName);
            if (data != null)
            {
                skillDataDict[skillName] = data;
            }
        }
    }

    public void RegisterSkill(string skillName, Skill skill, SkillInitializer initializer)
    {
        skills[skillName] = skill;
        initializers[skillName] = initializer;
    }

    public void InitializeSkill(string skillName)
    {
        if (initializers.TryGetValue(skillName, out var initializer) && skillDataDict.TryGetValue(skillName, out var data))
        {
            initializer.Initialize(data);
            currentSkill = skills[skillName];
            currentSkillName = skillName;
        }
        else
        {
            Debug.LogError($"Initializer or Skill data not found for skill: {skillName}");
        }
    }

    public Skill GetSkill(string skillName)
    {
        if (skills.TryGetValue(skillName, out var skill))
        {
            return skill;
        }

        return null;
    }

    public void ChangeSkill(string newSkillName)
    {
        if (skillDataDict.TryGetValue(newSkillName, out var skillData) && initializers.TryGetValue(newSkillName, out var initializer))
        {
            // 기존 이벤트 해제
            if (currentSkill != null && initializers.TryGetValue(currentSkillName, out var currentInitializer))
            {
                currentInitializer.UnregisterEvents();
            }

            // 기존 컴포넌트 제거 및 데이터 초기화
            currentSkill.ClearComponents();
            initializer.Initialize(skillData);  // 새로운 스킬 초기화

            // 새로운 이벤트 등록
            initializer.RegisterEvents();

            // 스킬 이름 변경
            currentSkill.gameObject.name = newSkillName;

            // 현재 스킬 정보 업데이트
            currentSkillName = newSkillName;
        }
    }

    public string GetCurrentSkillName()
    {
        return currentSkillName;
    }
}

/*
public void InitializeSkills()
{
    foreach (var skillName in skills.Keys)
    {
        if (initializers.TryGetValue(skillName, out var initializer))
        {
            initializer.Initialize(skillDataManager);
        }
    }
}
*/


/*
 * //SkillManager를 MonoBehaviour로 할 이유 없음.
    이 코드들은 스킬이 업데이트 될 때(데이터가 바뀐다거나(데미지가 150%에서 170%로 증가) 플레이어의 Stat이 바뀐다거나(공격력이 10에서 15로 증가))
    에도 바로바로 값을 적용시킬 수 있어? 아니면 기능을 추가하거나 기존의 내용을 좀 변경해야 해?

7. 스킬은 사용하는 도중에 지속적으로 데미지를 주는 공격이나 여러번 때리는 공격과 같은 경우에는 실시간으로 플레이어의 공격력 값이 변경될 수 있을 때 바로바로 적용을 시켜야 하잖아. 이런 경우에는 플레이어의 Stat에 관련된 내용이랑 계산을 해서 값이 변할때마다 최종 결과물을 도출해야 할거 같은데


1. 스킬들의 종류가 여러개야. 그런 경우에는 생성하고 초기화 하는 시점에서 너가 작성해준 GameInitializer 클래스의 skillDataSO도 여러개일거고, SkillDataEx data 도 여러개일거잖아. 이런 경우에는 List와 같은 배열들을 저장하는 자료구조를 이용해서 해야 할거 같은데 어떻게 생각해? 
2. SkillManager 클래스에서는 Initialize는 어디에서 실행하는거야? 
3. 그리고 아까전에도 이야기했지만 CreateSkill이라고 해서 Prefab을 따로 만들거나 하지는 않았어. 물론 Prefab을 사용하는 스킬이 있을수도 있지만 없는 스킬도 있을 테니까 말이야. 

2. 지금은 SkillComponent에서 event를 해제하는게 OnDestroy에서 수행되고 있는데 ...

4. HandleEnter는 현재 skill.OnEnter += HandlerEnter로 실행중인데, 
*/