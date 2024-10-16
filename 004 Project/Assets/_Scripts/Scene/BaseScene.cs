using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
	void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{ }
}
