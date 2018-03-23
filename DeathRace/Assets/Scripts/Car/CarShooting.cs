using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShooting : MonoBehaviour {

  public int m_PlayerNumber = 1;
  public CarMovement m_MovementScript;
  public Rigidbody m_Shell;
  public Transform m_LeftGun;
  public Transform m_RightGun;
  public float m_ShellVelocity = 5f;
  /*
  public AudioSource m_ShootingAudio;
  public AudioClip m_ChargingClip;
  public AudioClip m_FireClip;
  */
  public float m_Cooldown = 0.1f;

  private WaitForSeconds m_FireWait;
  private string m_fireButton;
  private bool m_fired = false;

  private void Start()
  {
    m_fireButton = "Fire" + m_PlayerNumber;

    m_FireWait = new WaitForSeconds(m_Cooldown);
  }

  public void Update()
  {
    if (Input.GetButton(m_fireButton))
    {
      Fire();
    }
  }

  private void Fire()
  {
    if(!m_fired)
    {
      Rigidbody shellInstanceL = Instantiate(m_Shell, m_LeftGun.position, m_LeftGun.rotation) as Rigidbody;
      Rigidbody shellInstanceR = Instantiate(m_Shell, m_RightGun.position, m_RightGun.rotation) as Rigidbody;

      shellInstanceL.velocity = (m_ShellVelocity+m_MovementScript.Velocity) * m_LeftGun.forward;
      shellInstanceR.velocity = (m_ShellVelocity+m_MovementScript.Velocity) * m_RightGun.forward;

      m_fired = true;

      StartCoroutine(FireCooldown());
    }
  }

  public IEnumerator FireCooldown()
  {
    yield return m_FireWait;

    m_fired = false;
  }
}
