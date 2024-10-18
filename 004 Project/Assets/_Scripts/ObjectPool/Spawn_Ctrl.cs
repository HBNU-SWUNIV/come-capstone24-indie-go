using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Ctrl : MonoBehaviour
{
    public int start,end = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i = start;i<end;i++)
            {
                ObjectPool.instance.monster[i].SetActive(true);
            }
        }
    }
}
