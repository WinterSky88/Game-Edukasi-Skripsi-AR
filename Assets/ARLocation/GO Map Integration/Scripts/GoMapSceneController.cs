using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMapSceneController : MonoBehaviour
{
    private ARLocationGoMapIntegration _goMap;
    
    void Start()
    {
	_goMap = FindObjectOfType<ARLocationGoMapIntegration>();
    }

    public void OnArButtonPress()
    {
	SceneManager.LoadScene(_goMap.ArSceneName, LoadSceneMode.Single);
    }
}
