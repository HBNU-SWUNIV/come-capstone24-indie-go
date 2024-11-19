using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public Text youDiedText;
    public Text pressAnyKeyText;
    public Image backgroundOverlay;
    private bool isGameOver = false;

    private void Update()
    {
        if (isGameOver && Input.anyKeyDown)
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);

            if (youDiedText != null)
            {
                youDiedText.text = "You Died";
                youDiedText.gameObject.SetActive(true);
            }

            if (pressAnyKeyText != null)
            {
                pressAnyKeyText.text = "Press Any Key";
                pressAnyKeyText.gameObject.SetActive(true);
            }

            if (backgroundOverlay != null)
            {
                backgroundOverlay.color = new Color(0, 0, 0, 0.2f); // 검은색, 50% 투명도
                backgroundOverlay.gameObject.SetActive(true);
            }

            isGameOver = true;
        }
        else
        {
            Debug.LogWarning("GameOverUI가 연결되지 않았습니다.");
        }
    }
}
