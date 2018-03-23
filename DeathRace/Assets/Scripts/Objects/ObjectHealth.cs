using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour {

  public float m_MaxHealth = 100f;

  private float m_currentHealth;
  private bool m_dead;

  public void OnEnable()
  {
    m_currentHealth = m_MaxHealth;
    m_dead = false;
  }

  public void TakeDamage(float amount)
  {
    m_currentHealth -= amount;

    if (m_currentHealth <= 0 && !m_dead)
    {
      OnDeath();
    }
  }

  public void OnDeath()
  {
    m_dead = true;

    gameObject.SetActive(false);
  }
}
