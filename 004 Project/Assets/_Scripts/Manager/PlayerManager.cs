using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
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
        player = GameObject.Find("Player");

        if (player == null)
            player = GameManager.Resource.Instantiate("Player");


        PlayerDataCollect = new PlayerDataCollect();
        DataAnalyze = new PlayerDataAnalyze();
        Camera.main.gameObject.GetComponent<MainCameraController>().SetPlayer(player);
    }

    private void SetSkills()
    {
        skillSetup = new SkillSetup(player);
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
