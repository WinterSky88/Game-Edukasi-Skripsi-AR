using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSceneButtonController : MonoBehaviour
{
    public void OnClick()
    {
	var g = FindObjectOfType<ARLocationGoMapIntegration>();
	g.OnMapButtonClick();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
