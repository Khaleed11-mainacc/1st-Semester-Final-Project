using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Hit Reaction")]
    [SerializeField] private float invincibleTime = 1.2f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 4f);
    [SerializeField] private float knockbackTime = 0.2f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [Tooltip("Script gerak/input player yang dimatikan saat knockback dan mati")]
    [SerializeField] private MonoBehaviour[] movementScripts;

    [Header("Game Over")]
    [SerializeField] private float gameOverDelay = 1.2f;

    public event Action<int, int> OnHealthChanged;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get { return maxHealth; } }
    public bool IsDead { get; private set; }

    private bool isInvincible;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount, Vector2 sourcePosition)
    {
        if (IsDead || isInvincible) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        if (OnHealthChanged != null) OnHealthChanged(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        SetTrigger("Hit");
        StartCoroutine(HitRoutine(sourcePosition));
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        if (OnHealthChanged != null) OnHealthChanged(CurrentHealth, maxHealth);
    }

    // Dipakai nanti buat jatuh ke air
    public void Kill()
    {
        if (IsDead) return;
        CurrentHealth = 0;
        if (OnHealthChanged != null) OnHealthChanged(CurrentHealth, maxHealth);
        Die();
    }

    private void Die()
    {
        StopAllCoroutines();
        IsDead = true;
        spriteRenderer.enabled = true;
        SetScripts(false);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        SetTrigger("Death");
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator HitRoutine(Vector2 sourcePosition)
    {
        isInvincible = true;
        SetScripts(false);

        float dir = Mathf.Sign(transform.position.x - sourcePosition.x);
        if (dir == 0f) dir = 1f;
        rb.linearVelocity = new Vector2(dir * knockbackForce.x, knockbackForce.y);

        yield return new WaitForSeconds(knockbackTime);
        SetScripts(true);

        float elapsed = 0f;
        while (elapsed < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        GameManager.Instance.GameOver();
    }

    private void SetScripts(bool enabledState)
    {
        foreach (MonoBehaviour script in movementScripts)
        {
            if (script != null) script.enabled = enabledState;
        }
    }

    private void SetTrigger(string triggerName)
    {
        if (animator == null) return;
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == triggerName)
            {
                animator.SetTrigger(triggerName);
                return;
            }
        }
    }

#if UNITY_EDITOR
    // Tes sementara: tekan H buat kena damage. Hapus setelah musuh jadi.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage(1, transform.position + Vector3.left);
    }
#endif
}
