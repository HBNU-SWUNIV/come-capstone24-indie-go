using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ItemEft/Consumable/Health")]
public class ItemHeal : Use_Effect
{
    public int healingPoint = 0;
    public override bool ExecuteRole()
    {
        Debug.Log("Player Hp Add" + healingPoint);
        return true;
    }
}
