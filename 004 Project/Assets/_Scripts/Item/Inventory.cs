using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int Inventory_count= 0;
    public List<Item> items = new List<Item>();
    void Awake()
    {
        instance = this;
    }
    public void RemoveItem(int idx)
    {
        items.RemoveAt(idx);
        Inventory_count -= 1;
        InventoryUI.instance.slots[idx].RemoveSlotUI();
        InventoryUI.instance.Check_Inventory();
    }
    
}