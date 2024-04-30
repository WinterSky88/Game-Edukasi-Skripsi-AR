using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InRangeText;
    [SerializeField] private GameObject Minigames2;
    [SerializeField] private GameObject Minigames3;
    [SerializeField] private GameObject webmap1;
    [SerializeField] private GameObject imageuiar;
    [SerializeField] private GameObject sapigame3;
    [SerializeField] private GameObject Panduan;
    //[SerializeField] private GameObject TooFarText;
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
        InRangeText.SetActive(true);
        isUIPanelActive = true;
        }
    }

    public void DisplayPanel2()
    {
        if (isUIPanelActive == false)
        {
            Minigames2.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayPanel3()
    {
        if (isUIPanelActive == false)
        {
            Minigames3.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayImage3()
    {
        if (isUIPanelActive == false)
        {
            sapigame3.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayPanel4()
    {
        if (isUIPanelActive == false)
        {
            webmap1.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayImage4()
    {
        if (isUIPanelActive == false)
        {
            imageuiar.SetActive(true);
            isUIPanelActive = true;
        }
    }

    public void DisplayPanel5()
    {
        if (isUIPanelActive == false)
        {
            Panduan.SetActive(true);
            isUIPanelActive = true;
        }
    }
    public void CloseButtonClick()
    {
    InRangeText.SetActive(false);
    Minigames2.SetActive(false);
        Minigames3.SetActive(false);
        sapigame3.SetActive(false);
        webmap1.SetActive(false);
        imageuiar.SetActive(false);
        Panduan.SetActive(false);
        isUIPanelActive = false;
    }
}
