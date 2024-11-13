using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ItemEft/Equip/Axe")]
public class ItemAtk : Use_Effect
{
    public int AtkPoint = 0;
    public override bool ExecuteRole()
    {
        GameManager.PlayerManager.Player.GetComponentInChildren<CharacterStats<PlayerStatsData>>().AddAttackDamage += AtkPoint;
        Debug.Log("Player Atk Add" + AtkPoint);

        return true;
    }
}
