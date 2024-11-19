using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataAnalyze : MonoBehaviour
{
    public string playerType;
    public float  parryRatio;
    public float  dashRatio;
    public float  runRatio;

    void Start()
    {
        // Analyze Playerdata
        // AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
    }

    public void AnalyzePlayerData(Dictionary<string, int> actionData)
    {
        // Calculate total Action
        int totalActions = actionData["ParryAttempt"] + actionData["DashAttempt"] + actionData["RunSuccess"];

        // Softmax calculation
        List<float> softmaxValues = Softmax(new List<float>
        {
            (float)actionData["ParryAttempt"] / totalActions,
            (float)actionData["DashAttempt"] / totalActions,
            (float)actionData["RunSuccess"] / totalActions
        });

        parryRatio = softmaxValues[0];
        dashRatio = softmaxValues[1];
        runRatio = softmaxValues[2];

        // Classify player style
        playerType = ClassifyPlayer(parryRatio, dashRatio, runRatio);

        // Debug output
        Debug.Log($"Parry Ratio = {parryRatio:F4}, Dodge Ratio = {dashRatio:F4}, Run Ratio = {runRatio:F4}, Play Style = {playerType}");
    }

    public List<float> Softmax(List<float> inputs)
    {
        float maxInput = Mathf.Max(inputs.ToArray());
        float sumExp = 0f;

        // Calculate exp for each input
        List<float> expValues = new List<float>();
        foreach (var input in inputs)
        {
            float expValue = Mathf.Exp(input - maxInput); // Numerical stability
            expValues.Add(expValue);
            sumExp += expValue;
        }

        // Normalize
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

        string detectedType = "Balanced";
        float maxRatio = -1f;

        // Find the maximum ratio and corresponding type
        foreach (var entry in ratios)
        {
            if (entry.Value > maxRatio)
            {
                maxRatio = entry.Value;
                detectedType = entry.Key;
            }
        }

        // Determine classification
        if (maxRatio > 0.5f) // High threshold
        {
            return $"High_{detectedType}";
        }
        else if (maxRatio > 0.4f) // Medium threshold
        {
            return detectedType;
        }
        else
        {
            return "Balanced"; // Fallback to Balanced
        }
    }
}
