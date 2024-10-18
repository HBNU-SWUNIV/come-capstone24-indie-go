using System.Collections;
using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{
    private bool setElement;
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
        StartCoroutine("InitializedEnemyElement");
        
        StartCoroutine("AdjustStatsBasedOnPlayerType");
    }

    private void OnDisable()
    {
        setElement = false;
    }
    private IEnumerator InitializedEnemyElement()
    {
        yield return null;
    /*    PlayerStats playerStats = GameManager.PlayerManager.Player.GetComponent<PlayerStats>();
        while (true)
        {
            if (playerStats.OnsetStats)
                break;
            yield return null;
        }
        Element playerElement = GameManager.PlayerManager.Player.GetComponent<PlayerStats>().Element;
        // playerElement에 따라서 확률로 결정. 불/얼음/대지/번개 중 불리 속성이 60%, 동일 속성이 20%, 유리 속성이 20%로 생성
        // 이는 인스펙터에서 확률을 변경할 수 있으면 좋겠음. 근데 이렇게 하려면 몬스터는 여러마리이니 별도의 클래스를 구현해야 할 듯. 
    */
        setElement = true;
    }

    private IEnumerator AdjustStatsBasedOnPlayerType()
    {
        while(true)
        {
            if (OnsetStats && setElement)
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
