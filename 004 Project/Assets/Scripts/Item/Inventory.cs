using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public GameObject Inventory_image;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
    public bool active;

#if UNITY_EDITOR
    private void OnValidate() {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake() {
        FreshSlot();
        Inventory_image.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("chk");
            active = !active;  
            if(active)
            {
                Inventory_image.gameObject.SetActive(true);
            }
            else
            {
                Inventory_image.gameObject.SetActive(false);
            }
        }
    }
    public void FreshSlot() {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++) {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++) {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item) {
        if (items.Count < slots.Length) {
            items.Add(_item);
            FreshSlot();
        } else {
            print("슬롯이 가득 차 있습니다.");
        }
    }
    
}