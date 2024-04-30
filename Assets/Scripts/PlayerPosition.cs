using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerPosition : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject cameraObject;
    void Update()
    {   
        Vector3 newPosition = new Vector3(cameraObject.transform.position.x,0.15f,cameraObject.transform.position.z);
        playerObject.transform.SetPositionAndRotation(newPosition,Quaternion.identity);
    }
}
