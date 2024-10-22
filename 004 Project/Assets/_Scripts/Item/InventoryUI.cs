using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public GameObject inventoryPanel;
    public List<Slot> slots = new List<Slot>();
    public bool activeInventory = false;
    void Awake()
    {
        instance = this;
    }
    void Start ()
    {
        inventoryPanel.SetActive(activeInventory);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            Check_Inventory();
            inventoryPanel.SetActive(activeInventory);
        }
        if(EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked on the UI");
        }
    }
    public void Check_Inventory()
    {
        for(int i =0; i< slots.Count;i++)
        {
            if(Inventory.instance.Inventory_count == i || Inventory.instance.items[i] == null){
                slots[i].RemoveSlotUI();
                break;
            }  
            slots[i].item = Inventory.instance.items[i]; 
            slots[i].UpdateSlotUI();
        }
    }
}