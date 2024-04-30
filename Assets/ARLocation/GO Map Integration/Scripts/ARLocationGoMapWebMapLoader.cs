using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using ARLocation;

public class ARLocationGoMapWebMapLoader : MonoBehaviour
{
    public PrefabDatabaseGoMap PrefabDatabase;
    public TextAsset XmlDataFile;
    public bool DebugMode;

    private ARLocationGoMapIntegration _goMapIntegration;
    private List<WebMapLoader.DataEntry> _dataEntries = new List<WebMapLoader.DataEntry>();
    private static ARLocationGoMapWebMapLoader _instance;

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

        DontDestroyOnLoad(gameObject);
    



    _goMapIntegration = GetComponent<ARLocationGoMapIntegration>();
	_goMapIntegration.OnGoMapInit += InitGoMapScene;
	_goMapIntegration.OnArSceneInit += InitArScene;
    }

    void InitGoMapScene()
    {
	Debug.Log("[WML]: InitGoMapScene");

	var _goMap = _goMapIntegration.GoMap;
	
	LoadXmlFile();
	foreach (var entry in _dataEntries)
	{
	    Debug.Log(entry.meshId);
	    var prefab = PrefabDatabase.GetDbEntryById(entry.meshId);
	    var instance = Instantiate(prefab.MapPinPrefab);
	    _goMap.dropPin(entry.lat, entry.lng, instance);
	    Debug.Log(instance.transform.position);
	}
    }

    void InitArScene()
    {
	var go = new GameObject("WebMapLoader");
	var wml = go.AddComponent<WebMapLoader>();
	wml.PrefabDatabase = PrefabDatabase.ToPrefabDb();
	wml.XmlDataFile = XmlDataFile;
	wml.DebugMode = DebugMode;
    }

    void LoadXmlFile()
        {
            var xmlString = XmlDataFile.text;

            Debug.Log(xmlString);

            XmlDocument xmlDoc = new XmlDocument();

            try {
                xmlDoc.LoadXml(xmlString);
            } catch(XmlException e) {
                Debug.LogError("[ARLocation#WebMapLoader]: Failed to parse XML file: " + e.Message);
            }

            var root = xmlDoc.FirstChild;
            var nodes = root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                Debug.Log(node.InnerXml);
                Debug.Log(node["id"].InnerText);

                int id = int.Parse(node["id"].InnerText);
                double lat = double.Parse(node["lat"].InnerText, CultureInfo.InvariantCulture);
                double lng = double.Parse(node["lng"].InnerText, CultureInfo.InvariantCulture);
                double altitude = double.Parse(node["altitude"].InnerText, CultureInfo.InvariantCulture);
                string altitudeMode = node["altitudeMode"].InnerText;
                string name = node["name"].InnerText;
                string meshId = node["meshId"].InnerText;
                float movementSmoothing = float.Parse(node["movementSmoothing"].InnerText, CultureInfo.InvariantCulture);
                int maxNumberOfLocationUpdates = int.Parse(node["maxNumberOfLocationUpdates"].InnerText);
                bool useMovingAverage = bool.Parse(node["useMovingAverage"].InnerText);
                bool hideObjectUtilItIsPlaced = bool.Parse(node["hideObjectUtilItIsPlaced"].InnerText);

                var entry = new WebMapLoader.DataEntry() {
                    id = id,
                    lat = lat,
                    lng = lng,
                    altitudeMode = altitudeMode,
                    altitude = altitude,
                    name = name,
                    meshId = meshId,
                    movementSmoothing = movementSmoothing,
                    maxNumberOfLocationUpdates = maxNumberOfLocationUpdates,
                    useMovingAverage =useMovingAverage,
                    hideObjectUtilItIsPlaced = hideObjectUtilItIsPlaced };

                _dataEntries.Add(entry);

                Debug.Log($"{id}, {lat}, {lng}, {altitude}, {altitudeMode}, {name}, {meshId}, {movementSmoothing}, {maxNumberOfLocationUpdates}, {useMovingAverage}, {hideObjectUtilItIsPlaced}");
            }
        }
}
