using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARLocation;

public class GoMapPlaceAtLocations : MonoBehaviour
{
    public List<PlaceAtLocation.LocationSettingsData> Locations;
    public PlaceAtLocation.PlaceAtOptions PlacementOptions;
    public GameObject Prefab;
    public GameObject MapPinPrefab;

    public bool DebugMode;

    private static GoMapPlaceAtLocations _instance;
    private ARLocationGoMapIntegration _goMapIntegration;

    void Awake()
    {
	if (!_instance)
	{
	    _instance = this;
	}
	else
	{
	    Destroy(gameObject);
	}

	_goMapIntegration = GetComponent<ARLocationGoMapIntegration>();
	_goMapIntegration.OnGoMapInit += InitGoMapScene;
	_goMapIntegration.OnArSceneInit += InitArScene;
    }

    void InitGoMapScene()
    {
	var goMap = _goMapIntegration.GoMap;
	foreach (var location in Locations)
	{
	    var instance = Instantiate(MapPinPrefab);
	    var loc = location.GetLocation();
	    Debug.Log($"loc = {loc}");
	    Debug.Assert(goMap);
	    goMap.dropPin(loc.Latitude, loc.Longitude, instance);
	}
    }

    void InitArScene()
    {
	var go = new GameObject("ARLocation GoMap PlaceAtLocations");

	var pal = go.AddComponent<PlaceAtLocations>();
	pal.Locations = Locations;
	pal.PlacementOptions = PlacementOptions;
	pal.Prefab = Prefab;
    }
}
