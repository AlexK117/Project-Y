using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSkript : MonoBehaviour {
    
    public Transform Car1; //not working because yu dou not Transform CloneCar maboi

    void LateUpdate ()
    {
        Vector3 newPosition = Car1.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
