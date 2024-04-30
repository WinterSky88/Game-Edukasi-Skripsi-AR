using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectSpawner : MonoBehaviour
{

    public GameObject clickablePrefab;
    public GameObject panelPrefab;

    void SpawnClickableObject()
    {
        GameObject clickableObject = Instantiate(clickablePrefab);
        ClickableObject clickableObjectComponent = clickableObject.GetComponent<ClickableObject>();

        GameObject panel = Instantiate(panelPrefab);
        clickableObjectComponent.SetPanelToActivate(panel);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
