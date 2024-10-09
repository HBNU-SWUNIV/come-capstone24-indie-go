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
            initializer.Initialize(data, skillName);
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
            initializer.Initialize(skillData, newSkillName);  // 새로운 스킬 초기화

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


