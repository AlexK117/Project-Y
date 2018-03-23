using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
  public Transform[] CameraPositions;

  private Camera thisCamera;
  private int counter;
  public void Start()
  {
    counter = 0;
    thisCamera = GetComponent<Camera>();
  }

  private void OnEnable()
  {
    transform.localPosition = CameraPositions[0].localPosition;
    transform.localRotation = CameraPositions[0].localRotation;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.H))
    {
      ChangeView();
    }

    if (Input.GetKeyDown(KeyCode.P))
    {
      ChangeSize();
    }
  }

  public void ChangeSize()
  {
    thisCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
  }

  private void ChangeView()
  {
    counter++;

    int view = counter % CameraPositions.Length;

    transform.localPosition = CameraPositions[view].localPosition;
    transform.localRotation = CameraPositions[view].localRotation;
  }
}
