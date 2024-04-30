using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardImage : MonoBehaviour
{
    public Sprite FrontTex, BackTex,DisableTex;
    public float R;
    public Vector3 v;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       v.y= Mathf.Lerp(v.y,R,Time.deltaTime*10);
        v.x = gameObject.GetComponent<RectTransform>().rotation.x;
        
        gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(v);
    }
    public void SwitchTex()
    {
        if(gameObject.GetComponent<Image>().sprite==BackTex)
        {
            R = 180;
            gameObject.GetComponent<Image>().sprite = FrontTex;
        }
        else
        {
            R = 0;
            gameObject.GetComponent<Image>().sprite = BackTex;
        }
    }
    public void DisableCards()
    {
        gameObject.GetComponent<Image>().sprite = DisableTex ;
    }
}
