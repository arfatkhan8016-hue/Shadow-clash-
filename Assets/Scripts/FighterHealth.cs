
using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log(gameObject.name + " defeated!");
        gameObject.SetActive(false);
    }
}
