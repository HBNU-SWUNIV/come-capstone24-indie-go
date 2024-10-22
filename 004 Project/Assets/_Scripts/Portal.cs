using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public GameObject text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) text.SetActive(true);
    }
    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(1);
            if(Input.GetKey(KeyCode.F))
                SceneManager.LoadScene(3);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) text.SetActive(false);
    }
}