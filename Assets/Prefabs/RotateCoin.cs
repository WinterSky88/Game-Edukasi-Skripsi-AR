using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    [SerializeField]
    private GameObject coin;

    // Update is called once per frame
    void Update()
    {
        coin.transform.Rotate(new Vector3(3.5f,0,0),Space.Self);
    }
}
