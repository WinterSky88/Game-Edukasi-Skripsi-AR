using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GameAndScanHandler : MonoBehaviour
{

    [SerializeField]
    private ARPlaneManager aRPlaneManager;

    private ARRaycastManager raycastManager;
    private GameObject spawnedObj;

    [SerializeField]
    private Text planeLevelText;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Text planeLevelTextEnd;

    [SerializeField]
    private Text endText;

    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject PlaceablePrefab;

    [SerializeField]
    private GameObject arrow;

    private bool ended = false;
    private bool resetted = false;

    private float resetTimeout = 2.0f;
    private List<GameObject> spawnedCoins;

    void Update()
    {
        if (resetted)
        {
            planeLevelText.text = "Area Livello 1";
            planeLevelText.color = Color.black;
            resetTimeout -= Time.deltaTime;
            if (resetTimeout < 0)
            {
                resetTimeout = 2.0f;
                resetted = false;
            }
        }
        else if (!ended)
        {
            arrow.transform.LookAt(getArrowTarget());
            arrow.transform.Rotate(new Vector3(85, 0, 0), Space.Self);
            if (spawnedObj != null)
                Debug.Log("Coin position --> " + spawnedObj.transform.position.ToString());

            if (GameStateKeeper.getInstance().getGameState() == GameStateKeeper.GameState.Scanning)
            {
                foreach (ARPlane plane in aRPlaneManager.trackables)
                {

                    Vector3 min = plane.gameObject.GetComponent<MeshFilter>().mesh.bounds.min;
                    Vector3 max = plane.gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
                    if (max.x > 1.6 && max.z > 1.6 && planeLevelText.text == "Area Livello 1")
                    {
                        planeLevelText.text = "Area Livello 2";
                        planeLevelText.color = Color.cyan;
                    }
                    if (max.x > 2.4 && max.z > 2.4 && planeLevelText.text == "Area Livello 2")
                    {
                        planeLevelText.text = "Area Livello 3";
                        planeLevelText.color = Color.blue;
                    }
                    if (max.x > 3 && max.z > 3 && planeLevelText.text == "Area Livello 3")
                    {
                        planeLevelText.text = "Area Livello 4";
                        planeLevelText.color = Color.magenta;
                    }
                    if (max.x > 3.5 && max.z > 3.5 && planeLevelText.text == "Area Livello 4")
                    {
                        planeLevelText.text = "Area Livello 5";
                        planeLevelText.color = Color.red;
                    }
                }
            }
            else if (GameStateKeeper.getInstance().getGameState() == GameStateKeeper.GameState.Ended)
            {
                Debug.Log("GAME ENDED");
                ended = true;
                if (GameStateKeeper.getInstance().getGameMode() == GameStateKeeper.GameMode.Timer)
                {
                    planeLevelTextEnd.text = planeLevelText.text;
                    planeLevelTextEnd.color = planeLevelText.color;
                    if(scoreText.text != "1")
                        endText.text = "Tempo Scaduto!\n\nHai raccolto\n" + scoreText.text + " Monete!";
                    else
                      endText.text = "Tempo Scaduto!\n\nHai raccolto\n" + scoreText.text + " Moneta!";
                }
                else
                {
                    planeLevelTextEnd.text = planeLevelText.text;
                    planeLevelTextEnd.color = planeLevelText.color;
                    endText.text = "Hai Finito!\n\nHai raccolto tutte le monete in " + timeText.text;
                }
            }
        }
    }


    void Awake()
    {
        spawnedCoins = new List<GameObject>();
        planeLevelText.text = "Area Livello 1";
        planeLevelText.color = Color.black;
        GameStateKeeper.getInstance().setGameState(GameStateKeeper.GameState.Welcome);
        raycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = aRPlaneManager.GetComponent<ARPlaneManager>();
        aRPlaneManager.enabled = false;
    }


    Vector3 CalculateNextPosition()
    {
        List<Vector3> possiblePositions = new List<Vector3>();
        Debug.Log("COIN - Trackable Planes found: " + aRPlaneManager.trackables.count.ToString());
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            Vector3 min = plane.gameObject.GetComponent<MeshFilter>().mesh.bounds.min;
            Vector3 max = plane.gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
            Debug.Log(plane.gameObject.transform.position.y);
            Vector3 position = plane.gameObject.transform.position - new Vector3((Random.Range(min.x * 0.80f, max.x * 0.80f)), plane.gameObject.transform.position.y * 0.03f, (Random.Range(min.z * 0.80f, max.z * 0.80f)));
            if (Vector3.Distance(player.transform.position, position) > 1.5)
                possiblePositions.Add(position);
        }
        if (possiblePositions.Count == 0)
        {
            Debug.Log("COIN - No possible positions found");
            return new Vector3(0, 0, 0);
        }
        int r = Random.Range(0, possiblePositions.Count);
        return possiblePositions[r];

    }


    public void TogglePlaneDetection()
    {
        ended = false;
        if (GameStateKeeper.getInstance().getGameState() != GameStateKeeper.GameState.Scanning)
            GameStateKeeper.getInstance().setGameState(GameStateKeeper.GameState.Scanning);
        else
        {
            GameStateKeeper.getInstance().setGameState(GameStateKeeper.GameState.Playing);
        }
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
        Debug.Log("TOGGLED PLANE DETECTION");

    }

    public void StartPlaying()
    {
        ended = false;
        GameStateKeeper.getInstance().setGameState(GameStateKeeper.GameState.Playing);
        Vector3 position = CalculateNextPosition();
        if (!position.Equals(new Vector3(0, 0, 0)))
        {
            spawnedObj = Instantiate(PlaceablePrefab, position, Quaternion.identity);
            spawnedCoins.Add(spawnedObj);
            position = CalculateNextPosition();
            spawnedObj = Instantiate(PlaceablePrefab, position, Quaternion.identity);
            spawnedCoins.Add(spawnedObj);
        }
        Debug.Log("START PLAYING");
    }

    public Transform getArrowTarget()
    {
        float distance = float.MaxValue;
        Transform toReturn = null;
        foreach (GameObject coin in spawnedCoins)
        {
            if (Vector3.Distance(coin.transform.position, player.transform.position) < distance)
            {
                distance = Vector3.Distance(coin.transform.position, player.transform.position);
                toReturn = coin.transform;
            }
        }
        if (toReturn != null)
            return toReturn;
        else
            return player.transform;
    }

    public void resetCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            coin.SetActive(false);
        }
        spawnedCoins.Clear();
    }


    public void resetPlanes()
    {
        resetted = true;
        arSession.Reset();
    }

}
