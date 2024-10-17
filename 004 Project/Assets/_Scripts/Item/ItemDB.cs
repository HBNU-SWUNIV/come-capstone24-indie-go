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
        
    }
}
