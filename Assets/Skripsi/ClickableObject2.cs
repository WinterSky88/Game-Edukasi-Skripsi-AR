using UnityEngine;
using GoShared;

public class ClickableObject2 : MonoBehaviour
{
    //private GameObject panelToActivate;
    //private bool panelActivated = false;
    UIManager UIManager;


    public void SetPanelToActivate(GameObject panel)
    {
        //panelToActivate = panel;
    }

    void Start()
    {
        UIManager = GameObject.Find("UI").GetComponent<UIManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //playerLocation = GameObject.Find("Canvas").GetComponent<LocationManager>();
            //var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLat(), playerLocation.GetLocationLon());
            //var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos.latitude, eventPos.longitude);
            //var distance = currentPlayerLocation.GetDistanceTo(eventLocation);
            //Debug.Log("Distance is: " + distance);
            //if (distance < 10000000)
            //{
                //UIManager.DisplayUserNotInRangePanel();
                //menuUIManager.DisplayStartEventPanel();


            //}
            //else
            //{
                //UIManager.DisplayStartEventPanel();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        // Perform actions when the object is clicked
                        Debug.Log("Clicked on the object!");
                        UIManager.DisplayPanel2();
                    }
                }
            }
        }
    }
