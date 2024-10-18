using System.Collections;
using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{
    [SerializeField] private int Exp;

    protected override void Awake()
    {
        base.Awake();
        //Element = Element.Land;
        //InitializeMonsterStats();
        // ChangeElement(Element.Land, landLevel);

        //animator = transform.root.GetComponent<Animator>();
    }


    private void OnEnable()
    {
        InitializeMonsterStats();
        // 플레이어 타입에 따른 스탯 조정        
        StartCoroutine("AdjustStatsBasedOnPlayerType");
    }

    private void OnDisable()
    {
        Debug.Log("몬스터 disable");
        // 몬스터 사망 시 플레이어에게 경험치 부여
        PlayerStats playerStats = GameManager.PlayerManager.Player.GetComponentInChildren<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.AddExp(Exp);
        }
        else
            Debug.Log("playerStats is null");

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
        ResetStatsToBaseValues();
        GameManager.PlayerManager.DataAnalyze.AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
        string playerType = GameManager.PlayerManager.DataAnalyze.playerType;
      //  Debug.Log($"GameManager.PlayerManager.DataAnalyze.playerType : {GameManager.PlayerManager.DataAnalyze.playerType}");
        switch (playerType)
        {
            case "High_parry":
            case "parry":
                // 플레이어가 패링을 선호하는 경우, 몬스터는 더 공격적으로 행동
                SetAdjustStatsAttackSpeed(1.2f); // 공격 속도 20% 증가
                SetAdjustStatsMoveSpeed(1.1f);   // 이동 속도 10% 증가
                break;

            case "High_dash":
            case "dash":
                // 플레이어가 회피를 선호하는 경우, 몬스터는 추적 능력 향상
                SetAdjustStatsAttackSpeed(0.8f); // 공격 속도 10% 감소
                SetAdjustStatsMoveSpeed(1.4f);   // 이동 속도 30% 증가
                break;

            case "High_run":
            case "run":
                // 플레이어가 도망을 선호하는 경우, 몬스터는 원거리 공격을 더 자주 사용
                SetAdjustStatsAttackSpeed(0.8f); // 공격 속도 20% 감소
                SetAdjustStatsMoveSpeed(1.2F);   // 이동 속도 20% 증가
                break;

            default:
                //임시
                SetAdjustStatsAttackSpeed(0.8f); // 공격 속도 20% 감소
                SetAdjustStatsMoveSpeed(1.2F);   // 이동 속도 20% 증가
                break;
        }
      //  Debug.Log($" playerType : {playerType} 에 해당하는 값으로 변경");
       // UpdateAnimatorSpeed();
    }



    private void InitializeMonsterStats()
    {
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
    protected override void SetStatsData(EnemyStatsData stats)
    {
        base.SetStatsData(stats);
        Exp = stats.Exp;
    }

}
