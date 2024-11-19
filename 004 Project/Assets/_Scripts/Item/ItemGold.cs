using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ItemEft/etc/Gold")]
public class ItemGold : Use_Effect
{
    public int GoldPoint = 0;
    public override bool ExecuteRole()
    {
        GameManager.PlayerManager.Player.GetComponentInChildren<CharacterStats<PlayerStatsData>>().Gold += GoldPoint;
        Debug.Log("Player Gold Add" + GoldPoint);
        return true;
    }
}
