using UnityEngine;

public class Coin : MonoBehaviour
{
    public CoinManager coinManager;
    public int value;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        coinManager.ChangeCoins(value);
        Destroy(gameObject);
    }
}
