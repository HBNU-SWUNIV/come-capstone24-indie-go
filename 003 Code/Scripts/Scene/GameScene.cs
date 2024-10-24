using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        GameManager.Instance.CreatePlayerManager();
        if(!GameObject.Find("Arrows"))
        {
            new GameObject
            {
                name = "Arrows"
            }.AddComponent<CoroutineHandler>();
        }
    }

}
