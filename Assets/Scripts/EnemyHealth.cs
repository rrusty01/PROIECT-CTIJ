using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBarFill;

        void Start()
        {
        currentHealth = maxHealth;
        UpdateHealthBar();
        }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Viata inamic: " + currentHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Inamicul a murit!");
        Destroy(gameObject);
    }
}
