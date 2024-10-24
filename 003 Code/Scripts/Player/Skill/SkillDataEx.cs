using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillDataEx
{
    private Dictionary<Type, SkillData> skillDataDict = new Dictionary<Type, SkillData>();

    public void AddData<T>(T data) where T : SkillData
    {
        skillDataDict[typeof(T)] = data;
    }

    public T GetData<T>() where T : SkillData
    {
        skillDataDict.TryGetValue(typeof(T), out SkillData data);
        return data as T;
    }
}