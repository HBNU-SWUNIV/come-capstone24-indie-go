using UnityEngine;
public abstract class SkillInitializer
{
    protected Skill skill;
    protected SkillGenerator generator;
    protected string skillName;
    protected GameObject prefab;
    protected Transform prefabParent;
    protected Transform playerTransform;
    protected Vector2 prefabOffset;
    public SkillInitializer(Skill skill, SkillGenerator generator, string skillName, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default(Vector2))
    {
        this.skill = skill;
        this.generator = generator;
        this.skillName = skillName;
        this.prefab = prefab;
        this.prefabParent = prefabParent;
        this.playerTransform = playerTransform;
        this.prefabOffset = prefabOffset;
    }

    public void Initialize(SkillDataEx data, string skillName)
    {
        // 스킬 데이터를 설정
        generator.InitializeSkill(skillName, skill, data, prefab);
        
        // 구체화된 스킬을 등록하고 이벤트 할당
        RegisterEvents();
    }

    public abstract void RegisterEvents();
    public abstract void UnregisterEvents();
}

public class SkillInitializer<TSkill, TGenerator> : SkillInitializer
    where TSkill : ISkillAction, new()
    where TGenerator : SkillGenerator
{
    public SkillInitializer(Skill skill, TGenerator generator, string skillName, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default(Vector2))
        : base(skill, generator, skillName, prefab, prefabParent, playerTransform, prefabOffset) {}

    public override void RegisterEvents()
    {
        TSkill concreteSkill = new TSkill();
        concreteSkill.Initialize(skill, prefab, prefabParent, playerTransform, prefabOffset);
        skill.OnEnter += concreteSkill.Enter;
        skill.OnExit += concreteSkill.Exit;
    }

    public override void UnregisterEvents()
    {
        TSkill concreteSkill = new TSkill();
        skill.OnEnter -= concreteSkill.Enter;
        skill.OnExit -= concreteSkill.Exit;
    }
}
