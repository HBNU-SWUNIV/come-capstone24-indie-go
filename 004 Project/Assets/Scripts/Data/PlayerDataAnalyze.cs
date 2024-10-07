using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataAnalyze : MonoBehaviour
{
    public static PlayerDataAnalyze instance;
    public PlayerDataCollect pdc;
    public string playerType;
    private void Awake()
    {
        playerType = "High_dash";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        if (pdc == null)
        {
            Debug.LogError("PlayerDataCollect instance is not assigned!");
            return;
        }

        // actionData bring Dictionary
        Dictionary<string, int> actionData = pdc.actionData;

        // Analyze Playerdata
        AnalyzePlayerData(actionData);
    }

    
    public void AnalyzePlayerData(Dictionary<string, int> actionData)
    {
        // calculaye total Action
        int totalActions = actionData["ParryAttempt"] + actionData["DashAttempt"] + actionData["DefenceAttempt"];

        // Logistic
        float parryRatio = LogisticFunction((float)actionData["ParryAttempt"] / totalActions);
        float dashRatio = LogisticFunction((float)actionData["DashAttempt"] / totalActions);
        float runRatio = LogisticFunction((float)actionData["DefenceAttempt"] / totalActions);

        // Ratio normalize
        float ratioSum = parryRatio + dashRatio + runRatio;
        parryRatio /= ratioSum;
        dashRatio /= ratioSum;
        runRatio /= ratioSum;

        // Classify player style
        string playStyle = ClassifyPlayer(parryRatio, dashRatio, runRatio);

        // print result
        Debug.Log($"Parry Ratio = {parryRatio:F4}, Dodge Ratio = {dashRatio:F4}, Run Ratio = {runRatio:F4}, Play Style = {playStyle}");
    }

    
    float LogisticFunction(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    
    public string ClassifyPlayer(float parryRatio, float dashRatio, float runRatio)
    {
        Dictionary<string, float> ratios = new Dictionary<string, float>
        {
            { "parry", parryRatio },
            { "dodge", dashRatio },
            { "run", runRatio }
        };

        
        float maxRatio = -1;

        foreach (var entry in ratios)
        {
            if (entry.Value > maxRatio)
            {
                maxRatio = entry.Value;
                playerType = entry.Key;
            }
        }

        switch (playerType)
        {
            case "parry":
                return maxRatio > 0.5f ? "High_parry" : maxRatio > 0.4f ? "parry" : "Balanced";
            case "dash":
                return maxRatio > 0.5f ? "High_dash" : maxRatio > 0.4f ? "dash" : "Balanced";
            case "run":
                return maxRatio > 0.5f ? "High_run" : maxRatio > 0.4f ? "run" : "Balanced";
            default:
                return "Balanced";
        }
    }
}