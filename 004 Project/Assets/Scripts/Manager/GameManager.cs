using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;
    public static GameManager Instance { get { Init(); return s_instance; } }
    

    DataManager data = new DataManager();
    ResourceManager resource = new ResourceManager();
    PlayerManager player;
    SharedCombatDataManager sharedData = new SharedCombatDataManager();
    ElementalManager elemental = new ElementalManager();
    public static DataManager Data { get{ return Instance.data; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    public static PlayerManager PlayerManager { get { return Instance.player; } }
    public static SharedCombatDataManager SharedCombatDataManager { get { return Instance.sharedData; } }
    public static ElementalManager ElementalManager { get { return Instance.elemental; } }
    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();

            s_instance.data.Init();
        }
        
    }

    public void CreatePlayerManager()
    {
        if (PlayerManager == null)
        {
            GameObject go = new GameObject("PlayerManager") { name = "@PlayerManager"};
            player = go.AddComponent<PlayerManager>();
            PlayerManager.Initialize();
        }
    }
}
