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
        // �÷��̾� Ÿ�Կ� ���� ���� ����
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
        // playerElement�� ���� Ȯ���� ����. ��/����/����/���� �� �Ҹ� �Ӽ��� 60%, ���� �Ӽ��� 20%, ���� �Ӽ��� 20%�� ����
        // �̴� �ν����Ϳ��� Ȯ���� ������ �� ������ ������. �ٵ� �̷��� �Ϸ��� ���ʹ� ���������̴� ������ Ŭ������ �����ؾ� �� ��. 
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
        // ������ �⺻ ������ ����
        ResetStatsToBaseValues();
        GameManager.PlayerManager.DataAnalyze.AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
        string playerType = GameManager.PlayerManager.DataAnalyze.playerType;
      //  Debug.Log($"GameManager.PlayerManager.DataAnalyze.playerType : {GameManager.PlayerManager.DataAnalyze.playerType}");
        switch (playerType)
        {
            case "High_parry":
            case "parry":
                // �÷��̾ �и��� ��ȣ�ϴ� ���, ���ʹ� �� ���������� �ൿ
                SetAdjustStatsAttackSpeed(1.2f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.1f);   // �̵� �ӵ� 10% ����
                break;

            case "High_dash":
            case "dash":
                // �÷��̾ ȸ�Ǹ� ��ȣ�ϴ� ���, ���ʹ� ���� �ɷ� ���
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 10% ����
                SetAdjustStatsMoveSpeed(1.4f);   // �̵� �ӵ� 30% ����
                break;

            case "High_run":
            case "run":
                // �÷��̾ ������ ��ȣ�ϴ� ���, ���ʹ� ���Ÿ� ������ �� ���� ���
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.2F);   // �̵� �ӵ� 20% ����
                break;

            default:
                //�ӽ�
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.2F);   // �̵� �ӵ� 20% ����
                break;
        }
      //  Debug.Log($" playerType : {playerType} �� �ش��ϴ� ������ ����");
       // UpdateAnimatorSpeed();
    }



    private void InitializeMonsterStats()
    {
        //id = 1001; // ������ ���� ID ����
        SetStat();
    }

    protected override void SetStat()
    {
        // id�� �´� ������ �˻�
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
        Id = newId; // ID�� �����ϰ�, �ڵ����� ������ �缳����
    }

 
}
