using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int totalCoins;
    public TMP_Text coinText;
    public GoalPoint goalPoint;

    void Start()
    {
        UpdateText();
    }

    public void ChangeCoins(int amount)
    {
        totalCoins += amount;
        UpdateText();
    }

    void UpdateText()
    {
        if (goalPoint != null)
            coinText.text = "Coins : " + totalCoins + "/" + goalPoint.goalCoins;
        else
            coinText.text = "Coins : " + totalCoins;
    }
}
