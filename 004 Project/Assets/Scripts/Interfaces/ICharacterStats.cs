using System;

public interface ICharacterStats
{
    event Action OnHealthZero;
    void DecreaseHealth(float amount);
}
