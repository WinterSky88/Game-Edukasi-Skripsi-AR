using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Card : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public GameObject selectedObject;
    public GameObject NewselectedObject;
    public bool canselectagain=true;
    public GameObject ImageObject;
    public GameObject LoseMenu, Winmenu;
    public int Chances;
    public int Matches,AllMatches;
    public TextMeshProUGUI Counters;
    public ImageAssigner imgassigner;
    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        
        Counters.text = "Sisa Percobaan:" + Chances + "               Benar: " + Matches + "/" + AllMatches;
        if (imgassigner.HasFlippedAll)
        {
            if (Chances == 0 && Matches < FindObjectOfType<ImageAssigner>().ccs.Length / 2)
            {
                LoseMenu.SetActive(true);
                Debug.Log("Lose");
            }

            if (Matches == AllMatches)
            {
                Winmenu.SetActive(true);
                Debug.Log("win");
            }
            // Check if there is a touch on the screen
            if (canselectagain)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // Set up the new Pointer Event
                    pointerEventData = new PointerEventData(eventSystem);
                    pointerEventData.position = Input.GetTouch(0).position;

                    // Raycast using the Graphics Raycaster and Pointer Event
                    List<RaycastResult> results = new List<RaycastResult>();
                    raycaster.Raycast(pointerEventData, results);

                    // Check if the raycast hits any objects
                    if (results.Count > 0)
                    {
                        if (selectedObject == null)
                        {

                            selectedObject = results[0].gameObject;
                            if (selectedObject.CompareTag("card"))
                            {
                                selectedObject.GetComponent<CardImage>().SwitchTex();
                            }
                            else
                            {
                                selectedObject = null;
                            }


                        }
                        else
                        {
                            if (selectedObject != results[0].gameObject)
                            {
                                NewselectedObject = results[0].gameObject;
                                NewselectedObject.GetComponent<CardImage>().SwitchTex();
                                canselectagain = false;

                            }
                            StartCoroutine(Showresult());
                        }

                    }
                }
            }
        }
    }
    IEnumerator Showresult()
    {
        yield return new WaitForSeconds(2);
        if (selectedObject.GetComponent<CardImage>().FrontTex == NewselectedObject.GetComponent<CardImage>().FrontTex)
        {

            Debug.Log("Victory");
            GameObject g = Instantiate(ImageObject,new Vector3(0,0,0),Quaternion.identity,transform);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            g.transform.localPosition = new Vector3(g.transform.localPosition.x,g.transform.localPosition.y,-0.45f);
            
            g.GetComponent<Image>().sprite = g.GetComponent<RightWrongImage>().Match;
            selectedObject.GetComponent<CardImage>().DisableCards();
            NewselectedObject.GetComponent<CardImage>().DisableCards();

            Matches++;
            selectedObject = null;
            NewselectedObject = null;
            
        }
        else
        {
            GameObject g = Instantiate(ImageObject, new Vector3(0, 0, 0), Quaternion.identity, transform);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            g.transform.localPosition = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, -0.45f);

            g.GetComponent<Image>().sprite = g.GetComponent<RightWrongImage>().NoMatch;
            Debug.Log("loss");
            selectedObject.GetComponent<CardImage>().SwitchTex();
            NewselectedObject.GetComponent<CardImage>().SwitchTex();
            selectedObject = null;
            NewselectedObject = null;
            
        }
        Chances--;
        canselectagain = true;
        
    }

    public void EnableCardSelection()
    {
        canselectagain = true;
    }
}
