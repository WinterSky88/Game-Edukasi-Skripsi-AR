using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageAssigner : MonoBehaviour
{
    public Sprite[] Sprites,Level1,Level2,Level3;
    public List<Image> Cards;
    public GameObject[] ccs;
    public Vector2[] positions;
    public Vector2 zeropos;
    public int CardCount;
    [SerializeField]int cardindex = 0;
    bool bSetpositions=false;
    public bool HasFlippedAll=false;
    public enum ImageType
    {
        First,
        Second,
        Third,
    }
    public bool FlipFlag = false;

    public ImageType Level_ImageType;
    private void Awake()
    {
        if(Level_ImageType==ImageType.First)
        {
            Sprites = new Sprite[Level1.Length];

            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = Level1[i];
            }

        }
        else if(Level_ImageType == ImageType.Second)
        {
            Sprites = new Sprite[Level2.Length];
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = Level2[i];
            }
        }
        else if(Level_ImageType==ImageType.Third)
        {
            Sprites = new Sprite[Level3.Length];
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = Level3[i];
            }
        }

        CardCount = Sprites.Length*2;
        for (int i = Cards.Count-1; i >= CardCount; i--)
        {
            Cards[i].gameObject.SetActive(false);
        }

        Cards.RemoveRange(CardCount,(Cards.Count)-(CardCount));
    }
    // Start is called before the first frame update
    void Start()
    {
       
        positions = new Vector2[Cards.Count];
        ccs = new GameObject[Cards.Count];
        for (int i = 0; i < Cards.Count; i++)
        {
            positions[i] = Cards[i].rectTransform.anchoredPosition;
            ccs[i] = Cards[i].gameObject;
        }
        
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprite s = Sprites[i]; //get an image from the pool
            for (int y = 0; y < 2; y++)
            {
                int c = Random.Range(0, Cards.Count); //select a random card 
                Cards[c].GetComponent<CardImage>().FrontTex=s; //assign the image to the card
                Cards.Remove(Cards[c]);// remove the card from the pool

            }
            
        }
        for (int i = 0; i < ccs.Length; i++)
        {
            ccs[i].GetComponent<RectTransform>().anchoredPosition = zeropos;
            ccs[i].GetComponent<CardImage>().v = new Vector3(0, 0, 0);
        }
        StartCoroutine(SetCardPositions());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bSetpositions)
        {
            if (Vector2.Distance(ccs[cardindex].GetComponent<RectTransform>().anchoredPosition, positions[cardindex]) > 0.01f && cardindex < ccs.Length)
            {
                ccs[cardindex].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ccs[cardindex].GetComponent<RectTransform>().anchoredPosition, positions[cardindex], Time.deltaTime * 10f*(cardindex+1 )/2);
            }
            else
            {
                ccs[cardindex].GetComponent<Image>().sprite = ccs[cardindex].GetComponent<CardImage>().FrontTex;
                if (cardindex <= ccs.Length-1)
                    cardindex++;
            }
        }
        if(cardindex==ccs.Length-1&&!FlipFlag)
        {
            Invoke(nameof(FlipBack),2f);
            FlipFlag = true;
        }
        
    }
    public void FlipBack()
    {
        
        foreach (var item in ccs)
        {
            item.GetComponent<CardImage>().SwitchTex();
           
        }
        HasFlippedAll = true;
    }
    public void SetPositions()
    {
        bSetpositions=true;
    }
    IEnumerator SetCardPositions()
    {
        
           
        
         yield return null;
    }
}
