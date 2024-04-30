using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARObjectPlacement : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private Image touchImage;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isPlacementValid = false;
    private GameObject placedObject;
    public QuizManager quizmanager;
    private int localquestionindex =-1;
    private void Start()
    {
        quizmanager = FindFirstObjectByType<QuizManager>();
        Screen.SetResolution(1080,1920,FullScreenMode.Windowed);
    }
    private void Update()
    {
        if(objectPrefab!=quizmanager.CorrectPrefab)
        {
            objectPrefab = quizmanager.CorrectPrefab;
            PlaceObject();
        }
        

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                // Check if the touch is over the specified UI image
                if (IsTouchOverUI(Input.GetTouch(0).position))
                {
                    if (isPlacementValid)
                    {
                        if (placedObject != null)
                        {
                            PlaceObject();
                            // Change the position of the placed object
                            placedObject.transform.position = hits[0].pose.position;
                            placedObject.transform.rotation = hits[0].pose.rotation;
                        }
                        else
                        {
                            // Place a new object
                            PlaceObject();
                        }
                    }
                }
            }
        }

        UpdatePlacementIndicator();
    }

    private bool IsTouchOverUI(Vector2 touchPosition)
    {
        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Create a PointerEventData to pass to the event system
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;

        // Raycast from the touch position and check if it hits any UI objects
        EventSystem.current.RaycastAll(eventData, results);

        // Check if the touch hit the specified UI image
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == touchImage.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdatePlacementIndicator()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
            
            isPlacementValid = true;
        }
        else
        {
            placementIndicator.SetActive(false);
            isPlacementValid = false;
        }
        
    }

    private void PlaceObject()
    {
        if (placedObject != null)
        {
            Destroy(placedObject);
        }

        placedObject = Instantiate(objectPrefab, hits[0].pose.position, hits[0].pose.rotation);
    }
   
}
