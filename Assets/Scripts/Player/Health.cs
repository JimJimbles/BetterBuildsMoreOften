using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    
    public float maxHealth = 10f;
    public float currentHealth { get; set; }
    public bool invincible { get; set; }
    
    
    public bool isDead;
    
    public UnityAction OnDie;
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void Heal(float healAmount)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
    
    public void TakeDamage(float damage)
    {
        if (invincible)
            return;
        
        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        HandleDeath();
    }
    
    void HandleDeath()
    {
        if (isDead)
            return;

        // call OnDie action
        if (currentHealth <= 0f)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }
}
