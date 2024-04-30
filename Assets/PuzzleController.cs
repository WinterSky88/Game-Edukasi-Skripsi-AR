using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
public class PuzzleController : MonoBehaviour
{
    public UnityEvent Won;
    public Texture2D mainTexture;
    private Texture2D croppedTexture;
    private GameObject[,] quads;
    public Texture2D[] textures;
    private Texture2D[,] _textures;
    public int row, column;
    private int height, width;
    public GameObject prefab;
    private GameObject spawnPrefab;
    private Vector3 pos;
    private int texturesHeight,texturesWidth;
    private Color[] mainTexturePixels;
    private static int index;
    private static int counter;
    private Texture previousTexture;
    private GameObject previousQuad;
    private bool isStarted;
    private bool firstSelected;
    public InputField inputWidth;
    public InputField inputHeight;
    public GameObject generateUI;
    public bool setEditorNumbers;
    private void Start()
    {
        if (!setEditorNumbers)
        {
            generateUI.SetActive(true);
        }
        else
        {
            SetUp();
        }
    }
    public void Generate()
    {
        generateUI.SetActive(false);
        row = System.Convert.ToInt32(inputHeight.text);
        column = System.Convert.ToInt32(inputWidth.text);
        if(quads != null) { 
        for(int i = 0; i < quads.GetLength(0); i++)
            {
                for(int j = 0; j < quads.GetLength(1); j++)
                {
                    Destroy(quads[i, j]);
                }
            }
        }
        SetUp();
    }
    public void SetUp()
    {
        GetSizes();
        SplitTexture();
        SetTexture();
        CreateQuads();
        StartCoroutine(IWaitingUntilStart());
    }
    private void GetSizes()
    {
        height = mainTexture.height;
        width = mainTexture.width;
        texturesHeight = (int)(height / row);
        texturesWidth = (int)(width / column);
        textures = new Texture2D[(row * column)];
        _textures = new Texture2D[row, column];
        quads = new GameObject[row, column];
    }
    public void SplitTexture()
    {
        index = 0;
        for (int currentPosheight = height - texturesHeight; currentPosheight >= 0; currentPosheight -= texturesHeight)
        {
            for (int currentPoswidth = 0; currentPoswidth <= width - texturesWidth; currentPoswidth += texturesWidth)
            {
                mainTexturePixels = mainTexture.GetPixels(currentPoswidth, currentPosheight, texturesWidth, texturesHeight);
                croppedTexture = new Texture2D(texturesWidth, texturesHeight);
                croppedTexture.SetPixels(mainTexturePixels);
                croppedTexture.Apply();
                textures[index] = croppedTexture;
                index++;
            }
        }
    }
    public void CreateQuads()
    {
        //How long the width of quad should be
        float scalex = prefab.transform.localScale.x / column;
        //How long the height of quad should be
        float scaley = prefab.transform.localScale.y / row;
        //To find the position of quads and sort them we should know at least one local position
        float posx = prefab.transform.position.x;
        float posy = prefab.transform.position.y + (scaley * column / 2);
        float posz = prefab.transform.position.z;
        Vector3 scale = new Vector3(scalex, scaley, 1);
        for (int i = 0; i < row; i++)
        {
            for (int k = 0; k < column ; k++)
            {
                //To sort the first quad
                if (spawnPrefab == null)
                {
                    pos = new Vector3(posx - (scalex * row / 2), posy, posz);
                    posx = posx - (scalex * row / 2);
                    
                }
                //The other ones we are sorting as sorted the first one
                else
                {
                    pos = new Vector3(posx + scalex, posy, posz);
                    posx += scalex;
                }
                //Instantiating quads
                spawnPrefab = Instantiate(prefab, transform);
                spawnPrefab.transform.localPosition = pos;
                spawnPrefab.transform.localScale = scale;
                quads[i, k] = spawnPrefab;
                quads[i, k].GetComponent<MeshRenderer>().material.mainTexture = _textures[i, k];
            }
            posy = posy - scaley;
            posx = prefab.transform.position.x;
            spawnPrefab = null;
        }
    }
    public void SetTexture()
    {
        //Making one size array to 2 size array
        index = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                _textures[i, j] = textures[index];
                index++;
            }
        }
    }
    public void ChangeTextures()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag == "quad")
                {
                   
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                    if (!firstSelected)
                    {
                        previousTexture = hit.transform.GetComponent<MeshRenderer>().material.mainTexture;
                        previousQuad = hit.transform.gameObject;
                        previousQuad.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        firstSelected = true; 
                    }
                    else 
                    {
                        previousQuad.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                        previousQuad.GetComponent<MeshRenderer>().material.mainTexture = hit.transform.GetComponent<MeshRenderer>().material.mainTexture;
                        hit.transform.GetComponent<MeshRenderer>().material.mainTexture = previousTexture;
                        CheckFields();
                        firstSelected = false;              
                    }
                }
            }
        }

    }
    public void CheckFields()
    {
        for(int i = 0; i < quads.GetLength(0); i++)
        {
            for(int j = 0; j < quads.GetLength(1); j++)
            {
                Debug.Log(i);
                if(quads[i,j].GetComponent<MeshRenderer>().material.mainTexture == _textures[i, j])
                {
                    counter++;
                    Debug.Log(counter);
                }
            }
        }
        if (counter == index)
        {
            Debug.Log("You win");
            isStarted = false;
            Won.Invoke();
        }
        counter = 0;
    }
    private void RandomizeTextures()
    {
        System.Random rand = new System.Random();
        List<int> listNumbers = new List<int>();
        do
        {
            int number = rand.Next(0, index);
            if (!listNumbers.Contains(number))
            {
                listNumbers.Add(number);
            }
        } while (listNumbers.Count < index);
        int index1 = 0;

        for (int i = 0; i < quads.GetLength(0); i++)
        {
            for (int j = 0; j < quads.GetLength(1); j++)
            {
                quads[i, j].GetComponent<MeshRenderer>().material.mainTexture = textures[listNumbers[index1]];
                index1++;
            }

        }
    }
    IEnumerator IWaitingUntilStart()
    {
        yield return new WaitForSeconds(2f);
        RandomizeTextures();
        isStarted = true;
    }
    private void Update()
    {
        if (isStarted)
        {
            ChangeTextures();
        }
    }
   
}
