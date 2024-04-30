using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.IO;

public class CoinCollector : MonoBehaviour
{
    public AudioSource coinSound;

    [SerializeField]
    private ARPlaneManager planeManager;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject hintArrow;

    public Text textScore;

    private int score = 0;

    private const int toCollect = 8;

    private float hintTimeout = 10.0f;


    void Awake()
    {
        hintArrow.SetActive(false);
    }

    void Update()
    {
        if (GameStateKeeper.getInstance().getGameState() == GameStateKeeper.GameState.Playing)
        {
            hintTimeout -= Time.deltaTime;
            if (hintTimeout < 0 && !hintArrow.activeSelf)
                toggleHintArrow();
        }
        else
        {
            hintArrow.SetActive(false);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (GameStateKeeper.getInstance().getGameState() == GameStateKeeper.GameState.Playing)
            if (collision.collider.tag == "Coin")
            {
                hintTimeout = 10.0f;
                if (hintArrow.activeSelf)
                {
                    toggleHintArrow();
                }
                Debug.Log("COIN COLLECTED");
                collision.gameObject.transform.position = CalculateNextPosition();
                Debug.Log("COIN - New Position --> " + collision.gameObject.transform.position.ToString());
                score++;
                textScore.text = score.ToString();
                coinSound.Play();

                if (GameStateKeeper.getInstance().getGameMode() == GameStateKeeper.GameMode.Coins && score == toCollect)
                {
                    GameStateKeeper.getInstance().setGameState(GameStateKeeper.GameState.Ended);
                }
            }
    }

    Vector3 CalculateNextPosition()
    {
        List<Vector3> possiblePositions = new List<Vector3>();
        planeManager = planeManager.GetComponent<ARPlaneManager>();
        Debug.Log("COIN - Trackable Planes found: " + planeManager.trackables.count.ToString());
        foreach (ARPlane plane in planeManager.trackables)
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

    public void toggleHintArrow()
    {
        if (hintArrow.activeSelf)
        {
            hintArrow.SetActive(false);
        }
        else
        {
            hintArrow.SetActive(true);
        }
    }

    public void resetArrowAndScore()
    {
        hintArrow.SetActive(false);
        hintTimeout = 10.0f;
        score = 0;
        textScore.text = "0";
    }

    public void ShareContent()
    {
        StartCoroutine(TakeScreenshotAndShare());
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "moneyGrabberTmp.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        Destroy(ss);

        if (GameStateKeeper.getInstance().getGameMode() == GameStateKeeper.GameMode.Timer)
            new NativeShare().AddFile(filePath)
                .SetSubject("Money Grabber AR").SetText("Ho totalizzato un punteggio di " + score.ToString() + " su Money Grabber AR, prova anche tu!")
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
        else
        {
            new NativeShare().AddFile(filePath)
        .SetSubject("Money Grabber AR").SetText("Ho preso tutte le monete su Money Grabber AR, prova anche tu!")
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();
        }
    }

}
