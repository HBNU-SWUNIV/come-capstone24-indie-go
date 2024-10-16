using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB instance;
    public GameObject fieldItemPrefab;
    public Vector3[] pos;
    public List<Item> itemDB = new List<Item>();
    void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        for(int i=0;i<20;i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0,3)]); 
        }
    }
}
