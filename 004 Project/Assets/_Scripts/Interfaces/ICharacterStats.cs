using System;

public interface ICharacterStats
{
    event Action OnHealthZero;
    int CurHp { get; set; }
    int MaxHp { get; set; }
    float AttackDamage { get; }
    float AttackSpeed { get; set; }
    float Defense { get; set; }
    float MoveSpeed { get; set; }
    bool isDead { get; set; }
    Element Element { get; set; }
    
    bool DecreaseHealth(float amount);
    void IncreaseHealth(float amount);
    bool IsHpMax(float amount);
    void ChangeDamage(float currentDamage);
    void ReturnDamage();
    void ChangeAttackSpeed(float currentSpeed);

    void ReturnAttackSpeed();
    public void ModifyAttackSpeed(float multiplier);
    public void ResetAttackSpeedSlow();
    public void ResetMoveSpeedSlow(ElementalComponent component);
    public void ApplyMoveSpeedSlow(float multiplier, ElementalComponent defenderComponent);
    public void ApplyAttackSpeedSlow(float multiplier);
}
