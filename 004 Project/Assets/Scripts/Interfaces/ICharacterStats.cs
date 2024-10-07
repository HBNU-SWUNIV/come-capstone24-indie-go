using System;

public interface ICharacterStats
{
    event Action OnHealthZero;
    int CurHp { get; set; }
    int MaxHp { get; set; }
    float AttackDamage { get; set; }
    float AttackSpeed { get; set; }
    float Defense { get; set; }
    float MoveSpeed { get; set; }
    Element Element { get; set; }
    bool DecreaseHealth(float amount);
    void IncreaseHealth(float amount);
    bool IsHpMax(float amount);
    void ChangeDamage(float currentDamage);
    void ReturnDamage();
    void ChangeAttackSpeed(float currentSpeed);
    void ChangeLandAttackSpeed(float currentSpeed);

    void ReturnAttackSpeed();
    void ReturnLandAttackSpeed();
    void ChangeMoveSpeed(float currentSpeed);
    void ReturnMoveSpeed();

}
