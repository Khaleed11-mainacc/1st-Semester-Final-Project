using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0.25f);

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += Refresh;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= Refresh;
    }

    private void Start()
    {
        Refresh(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void Refresh(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(i < max);
            hearts[i].color = i < current ? fullColor : emptyColor;
        }
    }
}
