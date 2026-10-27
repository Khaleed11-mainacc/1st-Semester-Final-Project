using UnityEngine;
using TMPro;

public class GoalPoint : MonoBehaviour
{
    public GameObject panel;
    public CoinManager coinManager;
    public int goalCoins;

    [Header("Panel Text")]
    public TMP_Text panelText;
    public string winText;
    public string loseText;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (coinManager.totalCoins >= goalCoins)
        {
            panelText.text = winText;
        }
        else
        {
            panelText.text = loseText;
        }
        panel.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        panel.SetActive(false);
    }
}
