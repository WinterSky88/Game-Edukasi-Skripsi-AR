using UnityEngine;
using System.Collections.Generic;
using GoMap;
using GoShared;

public class SpawnOnMap : MonoBehaviour
{
    [SerializeField]
    GOMap _map;

    [SerializeField]
    List<double> _latitudes;

    [SerializeField]
    List<double> _longitudes;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    float _interactionDistanceThreshold = 10f; // The maximum distance for player interaction

    [SerializeField]
    GameObject _markerPrefab;

    List<GameObject> _spawnedObjects;

    private Transform _playerTransform;

    private void Start()
    {
        _spawnedObjects = new List<GameObject>();
        int count = Mathf.Min(_latitudes.Count, _longitudes.Count);
        for (int i = 0; i < count; i++)
        {
            double latitude = _latitudes[i];
            double longitude = _longitudes[i];
            Coordinates coordinates = new Coordinates(latitude, longitude, 0);
            GameObject instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = coordinates.convertCoordinateToVector(instance.transform.position.y);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
        }

        // Find the player character by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerTransform = playerObject.transform;
        }
    }

    private void Update()
    {
        if (_playerTransform == null)
            return;

        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            var spawnedObject = _spawnedObjects[i];
            double latitude = _latitudes[i];
            double longitude = _longitudes[i];
            Coordinates coordinates = new Coordinates(latitude, longitude, 0);
            spawnedObject.transform.localPosition = coordinates.convertCoordinateToVector(spawnedObject.transform.position.y);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            // Calculate the distance between the player and the spawned object
            float distance = Vector3.Distance(spawnedObject.transform.position, _playerTransform.position);

            // Disable interaction if the distance exceeds the threshold
            bool canInteract = distance <= _interactionDistanceThreshold;
            spawnedObject.GetComponent<Collider>().enabled = canInteract;
        }
    }
}
