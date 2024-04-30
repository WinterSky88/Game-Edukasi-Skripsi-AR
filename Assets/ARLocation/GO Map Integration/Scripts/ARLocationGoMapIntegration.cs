using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using ARLocation;
using UnityEngine.UI;
using System;
public class ARLocationGoMapIntegration : MonoBehaviour

{
    public string GoMapSceneName;
    public string ArSceneName;
    public bool UseRawLocation;
    
    private static ARLocationGoMapIntegration _instance;
    private GoMap.GOMap _goMap;
    private ARLocationProvider _arLocationProvider;
    private MoveAvatar _moveAvatar;
    private ARLocationGoMapLocationManager _goLocationManager;
    private bool _goMapFirstLocationUpdate;
    private LoadOverlay _loadOverlay;

    public Action OnGoMapInit;
    public Action OnArSceneInit;

    public GoMap.GOMap GoMap => _goMap;
    
    void Awake()
    {
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		}
		void Start()
    {
	var currentScene = SceneManager.GetActiveScene();
	OnSceneLoaded(currentScene.name);

	SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
	SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
    }

    void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
	OnSceneLoaded(scene.name);
    }

    void SceneManagerOnSceneUnloaded(Scene scene)
    {
	if (scene.name == GoMapSceneName)
	{
	    OnGoMapSceneUnloaded();
	}
    }

    void OnSceneLoaded(string name)
    {
	if (name == GoMapSceneName)
	{
	    OnGoMapSceneLoaded();
	}
	else if (name == ArSceneName)
	{
	    OnArSceneLoaded();
	}
	else
	{
	    OnOtherSceneLoaded();
	}
    }

    void OnGoMapSceneLoaded()
    {
	Debug.Log("[ARLocationGoMapIntegration]: OnGoMapSceneLoaded");

	_loadOverlay = FindObjectOfType<LoadOverlay>();
	_arLocationProvider = GetComponent<ARLocationProvider>();
	_goMap = FindObjectOfType<GoMap.GOMap>();
	_moveAvatar = FindObjectOfType<MoveAvatar>();
	_goLocationManager = FindObjectOfType<ARLocationGoMapLocationManager>();
	_loadOverlay.Hide();
		

		_goMapFirstLocationUpdate = true;

	if (UseRawLocation)
	{
	    _arLocationProvider.OnRawLocationUpdated.AddListener(OnRawLocationUpdated);
	}
	else
	{
	    _arLocationProvider.OnLocationUpdated.AddListener(OnRawLocationUpdated);
	}

    }

    void OnRawLocationUpdated(Location location)
    {
	
	var coordinates = new GoShared.Coordinates(location.Latitude, location.Longitude);

	if (_goMapFirstLocationUpdate)
	{
	    _loadOverlay.Hide();
	    _goLocationManager.SetLocation(coordinates);
	    _goMapFirstLocationUpdate = false;
	    OnGoMapInit?.Invoke();
	}
	else
	{
	    _goLocationManager.ChangeLocation(coordinates);
	}
    }

    void OnGoMapSceneUnloaded()
    {
	Debug.Log("[ARLocationGoMapIntegration]: OnGoMapSceneUnloaded");
	if (UseRawLocation)
	{
	    _arLocationProvider.OnRawLocationUpdated.RemoveListener(OnRawLocationUpdated);
	}
	else
	{
	    _arLocationProvider.OnLocationUpdated.RemoveListener(OnRawLocationUpdated);
	}
    }

    void OnArSceneLoaded()
    {
	Debug.Log("[ARLocationGoMapIntegration]: OnArSceneLoaded");
	OnArSceneInit?.Invoke();
    }

    void OnOtherSceneLoaded()
    {
	//Destroy(gameObject);
    }

    public void OnMapButtonClick()
    {
	SceneManager.LoadScene(GoMapSceneName, LoadSceneMode.Single);
    }

    public void OnArButtonClick()
    {
	Debug.Log("AR BUTTON CLICK");
	SceneManager.LoadScene(ArSceneName, LoadSceneMode.Single);
    }
}
