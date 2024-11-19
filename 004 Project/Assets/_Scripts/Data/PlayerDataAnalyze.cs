using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataAnalyze : MonoBehaviour
{
    public string playerType;
    public float[] actionRatios = new float[3];
    public bool changePlayerType;
    private string currentPlayerType;
    void Start()
    {
        // Analyze Playerdata
        // AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
    }

    public void AnalyzePlayerData(Dictionary<string, int> actionData)
    {
        // 총 액션 횟수 계산
        int totalActions = actionData["ParryAttempt"] + actionData["DashAttempt"] + actionData["RunSuccess"];

        // 비율 계산
        
        actionRatios[0] = (float)actionData["ParryAttempt"] / totalActions;
        actionRatios[1] = (float)actionData["DashAttempt"] / totalActions;
        actionRatios[2] = (float)actionData["RunSuccess"] / totalActions;

        // Softmax 함수 적용
        float[] softmaxRatios = Softmax(actionRatios);
        string newPlayerType = ClassifyPlayer(softmaxRatios[0], softmaxRatios[1], softmaxRatios[2]);
        if (newPlayerType != currentPlayerType)
        {
            changePlayerType = true;
            currentPlayerType = newPlayerType;
        }
        else
        {
            changePlayerType = false;
        }
        playerType = newPlayerType;

        // 플레이어 스타일 분류
        

        // 결과 출력 (디버그용)
        Debug.Log($"Parry Ratio = {softmaxRatios[0]:F4}, Dash Ratio = {softmaxRatios[1]:F4}, Run Ratio = {softmaxRatios[2]:F4}, Play Style = {playerType}");
    }

    public float[] Softmax(float[] values)
    {
        float offset = Mathf.Max(values); // 가장 큰 값 사용 (값 튐 방지)
        float sumExp = 0f;
        float[] expValues = new float[values.Length];

        // 모든 값의 지수 계산 (값이 튀는 것을 방지하기 위해 offset 사용)
        for (int i = 0; i < values.Length; i++)
        {
            expValues[i] = Mathf.Exp(values[i] - offset);
            sumExp += expValues[i];
        }

        // 각 값에 대해 소프트맥스 계산
        for (int i = 0; i < values.Length; i++)
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
        if (maxRatio > 0.6f) // High threshold
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
