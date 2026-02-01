using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public float flashDuration = 0.1f;

    private int currentHealth;
    private float damageMultiplier = 1f;
    private bool isDead;

    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        int finalDamage = Mathf.CeilToInt(amount * damageMultiplier);
        if (finalDamage <= 0) return;

        currentHealth -= finalDamage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }

    void Die()
    {
        isDead = true;
        SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1f;
    }

    public bool IsDead => isDead;
    public int CurrentHealth => currentHealth;
}
