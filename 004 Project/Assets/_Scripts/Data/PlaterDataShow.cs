using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaterDataShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.label);
        myStyle.fontSize = 50;
        GUI.Label(new Rect(200,100,Screen.width*0.5f,Screen.height*0.25f),"PlayerType : " + GameManager.PlayerManager.DataAnalyze.playerType,myStyle);
    }
}
