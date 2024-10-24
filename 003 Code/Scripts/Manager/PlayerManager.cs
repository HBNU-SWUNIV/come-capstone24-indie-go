using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player { get; private set; }
    private SkillSetup skillSetup;

    public PlayerDataCollect PlayerDataCollect { get; private set; }
    public PlayerDataAnalyze DataAnalyze { get; private set; }

    public void Initialize()
    {
        CreatePlayer();
        SetSkills();
    }

    private void CreatePlayer()
    {
        Player = GameObject.Find("Player");

        if (Player == null)
            Player = GameManager.Resource.Instantiate("Player");


        PlayerDataCollect = new PlayerDataCollect();
        DataAnalyze = new PlayerDataAnalyze();
        Camera.main.gameObject.GetComponent<MainCameraController>().SetPlayer(Player);
    }

    private void SetSkills()
    {
        skillSetup = new SkillSetup(Player);
    }

    public Skill GetCurrentSkill()
    {
        return skillSetup.GetCurrentSkill();
    }

    public void ChangeSkill(Element newElement)
    {
        skillSetup.ChangeSkill(newElement);
    }
}
