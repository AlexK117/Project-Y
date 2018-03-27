using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

  public GameObject m_CarPrefab;
  public Camera MenuCamera;
  public CarManager[] m_Cars;
  
  

  private int amountOfPlayers;

  private void Awake()
  {

  }

  public void Set1Player()
  {

    m_Cars[0].m_Instance =
      Instantiate(m_CarPrefab, m_Cars[0].m_SpawnPoint.position, m_Cars[0].m_SpawnPoint.rotation) as GameObject;
    m_Cars[0].m_PlayerNumber = 1;
    m_Cars[0].Setup();

    MenuCamera.gameObject.SetActive(false);

    }

  public void Set2Players()
  {
  
    for (int i = 0; i < m_Cars.Length; i++)
    {
      m_Cars[i].m_Instance =
        Instantiate(m_CarPrefab, m_Cars[i].m_SpawnPoint.position, m_Cars[i].m_SpawnPoint.rotation) as GameObject;
      m_Cars[i].m_PlayerNumber = i + 1;
      m_Cars[i].Setup();
    }
    
    m_Cars[0].AdjustCamera(0, 0, 1, 0.5f);
    m_Cars[1].AdjustCamera(0, 0.5f, 1, 0.5f);
Debug.Log("lol");
    MenuCamera.gameObject.SetActive(false);
  }



}
