using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataAnalyze : MonoBehaviour
{
    public string playerType;

    void Start()
    {   
        // Analyze Playerdata
        AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
    }

    public void AnalyzePlayerData(Dictionary<string, int> actionData)
    {
        // Calculate total actions
        int totalActions = actionData["ParryAttempt"] + actionData["DashAttempt"] + actionData["RunSuccess"];

        if (totalActions == 0)
        {
            Debug.LogError("No actions detected.");
            return;
        }

        // Get the ratios of each action
        float parryRatio = (float)actionData["ParryAttempt"] / totalActions;
        float dashRatio = (float)actionData["DashAttempt"] / totalActions;
        float runRatio = (float)actionData["RunSuccess"] / totalActions;

        // Apply softmax function to normalize
        List<float> softmaxRatios = Softmax(new List<float> { parryRatio, dashRatio, runRatio });
        
        // Extract the ratios after softmax
        parryRatio = softmaxRatios[0];
        dashRatio = softmaxRatios[1];
        runRatio = softmaxRatios[2];

        // Classify player style based on softmax values
        string playStyle = ClassifyPlayer(parryRatio, dashRatio, runRatio);

        // Print result
        Debug.Log($"Parry Ratio = {parryRatio:F4}, Dash Ratio = {dashRatio:F4}, Run Ratio = {runRatio:F4}, Play Style = {playStyle}");
    }

    // Softmax function implementation
    public List<float> Softmax(List<float> values)
    {
        float maxVal = Mathf.Max(values.ToArray());  // Prevent overflow
        List<float> expValues = new List<float>();
        float sumExp = 0f;

        // Calculate exponentials and sum of exponentials
        foreach (var val in values)
        {
            float expVal = Mathf.Exp(val - maxVal);
            expValues.Add(expVal);
            sumExp += expVal;
        }

        // Normalize to get softmax probabilities
        for (int i = 0; i < expValues.Count; i++)
        {
            expValues[i] /= sumExp;
        }

        return expValues;
    }

    public string ClassifyPlayer(float parryRatio, float dashRatio, float runRatio)
    {
        Dictionary<string, float> ratios = new Dictionary<string, float>
        {
            { "parry", parryRatio },
            { "dash", dashRatio },
            { "run", runRatio }
        };

        // Identify the action with the highest ratio
        float maxRatio = -1;

        foreach (var entry in ratios)
        {
            if (entry.Value > maxRatio)
            {
                maxRatio = entry.Value;
                playerType = entry.Key;
            }
        }

        // Classify based on the highest action ratio
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
