using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArSceneController : MonoBehaviour
{
    private ARLocationGoMapIntegration _goMap;
    
    void Start()
    {
	_goMap = FindObjectOfType<ARLocationGoMapIntegration>();
    }

    public void OnArButtonPress()
    {
	SceneManager.LoadScene(_goMap.GoMapSceneName, LoadSceneMode.Single);
    }
}
