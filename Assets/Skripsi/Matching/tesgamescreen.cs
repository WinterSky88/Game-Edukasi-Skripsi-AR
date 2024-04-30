using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;

public class tesgamescreen : MonoBehaviour
{
    [SerializeField] private GameObject gameScreenPrefab;
    [SerializeField] private float gameScreenScaleFactor = 2f; // Adjust the scale factor as needed
    [SerializeField] private float smoothMoveDuration = 0.5f; // Adjust the smoothing duration as needed

    private ARPlaneManager planeManager;
    private Vector3 previousPosition;
    private bool gameScreenPlaced = false;
    private ARRaycastManager raycastManager;
    private Vector2 touchPosition;

    private void Awake()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
        planeManager.planesChanged += OnPlanesChanged;
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void OnDestroy()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void Update()
    {
        if (!gameScreenPlaced && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            TryPlaceGameScreen();
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        if (!gameScreenPlaced && eventArgs.added != null && eventArgs.added.Count > 0)
        {
            ARPlane plane = eventArgs.added[0];
            PlaceGameScreen(plane);
            gameScreenPlaced = true;

            // Disable the ARPlaneManager to prevent plane visualizer creation
            planeManager.enabled = false;
        }
    }

    private void TryPlaceGameScreen()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0)
        {
            ARPlane plane = hits[0].trackable as ARPlane;
            if (plane != null)
            {
                PlaceGameScreen(plane);
                gameScreenPlaced = true;

                // Disable the ARPlaneManager to prevent plane visualizer creation
                planeManager.enabled = false;
            }
        }
    }

    private void PlaceGameScreen(ARPlane plane)
    {
        Vector3 planeCenter = plane.center;
        Quaternion planeRotation = plane.gameObject.transform.rotation;

        GameObject gameScreen = Instantiate(gameScreenPrefab, planeCenter, planeRotation);
        gameScreen.transform.SetParent(plane.transform);
        gameScreen.transform.localScale = Vector3.one * gameScreenScaleFactor;

        // Disable the ARPlaneMeshVisualizer to hide the ARPlane line visualizer
        ARPlaneMeshVisualizer meshVisualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
        if (meshVisualizer != null)
        {
            meshVisualizer.enabled = false;
        }

        if (previousPosition != Vector3.zero)
        {
            // Smoothly move the game screen towards the new position
            StartCoroutine(SmoothMove(gameScreen.transform, previousPosition, planeCenter, smoothMoveDuration));
        }

        previousPosition = planeCenter;

        // Detach from the initial parent plane and attach to the AR session
        gameScreen.transform.SetParent(null);
    }

    private IEnumerator SmoothMove(Transform target, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            target.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        target.position = endPosition;
    }
}
