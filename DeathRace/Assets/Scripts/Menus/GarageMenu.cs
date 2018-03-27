using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageMenu : MonoBehaviour {

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + +1);
    }
}