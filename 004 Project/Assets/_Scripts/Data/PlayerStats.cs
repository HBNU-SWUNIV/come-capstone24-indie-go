using UnityEngine;

public class PlayerStats : CharacterStats<PlayerStatsData>
{
    [SerializeField] private int level;
    [SerializeField] private int MaxExp;
    private int currentExp;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            SetStat();
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SetStatsData(PlayerStatsData stats)
    {
        base.SetStatsData(stats);
        MaxExp = stats.MaxExp;
    }
    private void OnEnable()
    {
        InitializePlayerStats();

    }

    private void OnDisable()
    {
        if (isDead == true)
        {
            transform.root.GetComponent<CharacterAudio>().PlayDeathSound();
        }
    }
    private void Start()
    {
        ChangeElement(Element.Fire, fireLevel);
    }

    private void InitializePlayerStats()
    {
        // 여기서 id와 level을 초기화
        id = 1; // 플레이어의 고유 ID 설정
        level = 1; // 초기 레벨 설정
        currentExp = 0;
        SetStat();

    }

    protected override void SetStat()
    {
        // id와 level에 맞는 데이터 검색
        foreach (var kvp in GameManager.Data.PlayerStatsDict)
        {
            var stats = kvp.Value;
            if (stats.id == id && stats.level == level)
            {
                SetStatsData(stats);
                return;
            }
        }
        Debug.LogError("Failed to load player stats for id: " + id + " and level: " + level);
    }

    public void AddExp(int exp)
    {
        currentExp += exp;
        Debug.Log($"경험치 획득: {exp}, 현재 경험치: {currentExp}/{MaxExp}");

        if (currentExp >= MaxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentExp -= MaxExp; // 초과된 경험치는 다음 레벨로 이어짐
        Level++; // 레벨 증가
        Debug.Log($"레벨업! 현재 레벨: {Level}");
    }

    protected override void UpdateAnimatorMoveSpeed()
    {
        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", moveSpeed);
        }
    }
    protected override void UpdateAnimatorAttackSpeed()
    {
        if (animator != null)
        {
            GameObject sword = transform.parent.Find("Weapons").GetChild(0).gameObject;

            Animator swordbaseAnim = sword.transform.GetChild(0).GetComponent<Animator>();
            Animator swordweaponAnim = sword.transform.GetChild(1).GetComponent<Animator>();

            Debug.Log(attackSpeed);
            swordbaseAnim.SetFloat("AttackSpeed", attackSpeed);
            swordweaponAnim.SetFloat("AttackSpeed", attackSpeed);
        }
    }
    private void Update()
    {
        HandleElementChange();
    }
    private void HandleElementChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!transform.root.GetComponentInChildren<Skill>(true).gameObject.activeSelf)
            {
                ChangeElement(Element.Fire, fireLevel);
                Debug.Log("Player ChangeElement : Fire");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!transform.root.GetComponentInChildren<Skill>(true).gameObject.activeSelf)
            {
                ChangeElement(Element.Ice, iceLevel);
                Debug.Log("Player ChangeElement : Ice");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!transform.root.GetComponentInChildren<Skill>(true).gameObject.activeSelf)
            {
                ChangeElement(Element.Land, landLevel);
                Debug.Log("Player ChangeElement : Land");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!transform.root.GetComponentInChildren<Skill>(true).gameObject.activeSelf)
            {
                ChangeElement(Element.Light, lightLevel);
                Debug.Log("Player ChangeElement : Light");
            }
        }
    }




    public void IncreaseElementLevel(Element element)
    {
        int level = 0;
        switch (element)
        {
            case Element.Fire:
                fireLevel++;
                level = fireLevel;
                break;
            case Element.Ice:
                iceLevel++;
                level = iceLevel;
                break;
            case Element.Land:
                landLevel++;
                level = landLevel;
                break;
            case Element.Light:
                lightLevel++;
                level = lightLevel;
                break;
        }
        Debug.Log($"{element} level up");
        UpdateElementalEffect(element, level);
    }

    public int GetElementLevel(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                return fireLevel;
            case Element.Ice:
                return iceLevel;
            case Element.Land:
                return landLevel;
            case Element.Light:
                return lightLevel;
            default:
                return 1;
        }
    }
}
