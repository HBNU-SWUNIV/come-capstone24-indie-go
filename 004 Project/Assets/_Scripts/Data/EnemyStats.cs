using System.Collections;
using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{

    protected override void Start()
    {
        base.Start();
        //Element = Element.Land;
        InitializeMonsterStats();
        ChangeElement(Element.Land, landLevel);

        //animator = transform.root.GetComponent<Animator>();

    }

    private void OnEnable()
    {       
        
        // 플레이어 타입에 따른 스탯 조정
            StartCoroutine("AdjustStatsBasedOnPlayerType");
    }

    private IEnumerator AdjustStatsBasedOnPlayerType()
    {
        while(true)
        {
            if (OnsetStats)
                break;
            yield return null;
        }
        // 스탯을 기본 값으로 리셋
        //ResetStatsToBaseValues();
        GameManager.PlayerManager.DataAnalyze.AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
        string playerType = GameManager.PlayerManager.DataAnalyze.playerType;
        Debug.Log($"GameManager.PlayerManager.DataAnalyze.playerType : {GameManager.PlayerManager.DataAnalyze.playerType}");
        switch (playerType)
        {
            case "High_parry":
            case "parry":
                // 플레이어가 패링을 선호하는 경우, 몬스터는 더 공격적으로 행동
                IncreaseAttackSpeed(1.2f); // 공격 속도 20% 증가
                IncreaseMoveSpeed(1.1f);   // 이동 속도 10% 증가
                break;

            case "High_dash":
            case "dash":
                // 플레이어가 회피를 선호하는 경우, 몬스터는 추적 능력 향상
                IncreaseAttackSpeed(0.8f); // 공격 속도 10% 감소
                IncreaseMoveSpeed(1.4f);   // 이동 속도 30% 증가
                break;

            case "High_run":
            case "run":
                // 플레이어가 도망을 선호하는 경우, 몬스터는 원거리 공격을 더 자주 사용
                IncreaseAttackSpeed(0.8f); // 공격 속도 20% 감소
                IncreaseMoveSpeed(1.2F);   // 이동 속도 20% 증가
                break;

            default:
                break;
        }
        Debug.Log($" playerType : {playerType} 에 해당하는 값 변경");
    }


    private void IncreaseAttackSpeed(float multiplier)
    {
        attackSpeed *= multiplier;
        UpdateAnimatorAttackSpeed();
    }

    private void IncreaseMoveSpeed(float multiplier)
    {
        moveSpeed *= multiplier;
        UpdateAnimatorMoveSpeed();
    }
    private void InitializeMonsterStats()
    {
        //id = 1001; // 몬스터의 고유 ID 설정
        SetStat();
    }

    protected override void SetStat()
    {
        // id에 맞는 데이터 검색
        if (GameManager.Data.EnemyStatsDict.TryGetValue(id, out var stats))
        {
            SetStatsData(stats);
        }
        else
        {
            Debug.LogError("Failed to load monster stats for id: " + id);
        }
    }

    public void SetMonsterId(int newId)
    {
        Id = newId; // ID를 설정하고, 자동으로 스탯이 재설정됨
    }
}
