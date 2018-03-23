using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHealth : MonoBehaviour
{

  public float m_StartingHealth = 100f;

  private float m_currentHealth;
  private bool m_Dead;

  private void OnEnable()
  {
    m_currentHealth = m_StartingHealth;
    m_Dead = false;
  }

  public void TakeDamage(float amount)
  {
    m_currentHealth -= amount;

    if (m_currentHealth <= 0 && !m_Dead)
    {
      OnDeath();
    }
  }

  private void OnDeath()
  {
    m_Dead = true;

    gameObject.SetActive(false);
  }
}
