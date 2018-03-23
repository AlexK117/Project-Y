using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellInstance : MonoBehaviour
{

  public LayerMask m_PlayerMask;
  public LayerMask m_ObjectMask;
  //public AudioSource m_ExplosionAudio;
  public float m_Damage;
  public float m_MaxLifeTime = 1f;

  private void Start()
  {
    Destroy(gameObject, m_MaxLifeTime);
  }

  private void OnTriggerEnter(Collider other)
  {
    ObjectHealth targetHealth = other.GetComponent<ObjectHealth>();
    CarHealth carHealth = other.GetComponent<CarHealth>();

    if (targetHealth)
    {
      targetHealth.TakeDamage(m_Damage);
    }
    else if (carHealth)
    {
      carHealth.TakeDamage(m_Damage);
    }

    Destroy(gameObject);
  }
}
