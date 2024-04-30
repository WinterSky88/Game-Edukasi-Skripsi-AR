using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject eventPanelYa;
    [SerializeField] private GameObject eventPanelTidak;
    bool isUIPanelActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStartEventPanel()
    {
        if(isUIPanelActive == false)
        {
            eventPanelYa.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayUserNotInRangePanel()
    {
        if(isUIPanelActive == false)
        {
            eventPanelTidak.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void CloseButtonClick()
    {
        eventPanelYa.SetActive(false);
        eventPanelTidak.SetActive(false);
        isUIPanelActive = false;
    }
}
