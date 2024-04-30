using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreSetter : MonoBehaviour
{
    public TextMeshProUGUI AnimalScore, HumanScore, FruitScore;
    // Start is called before the first frame update
    void Start()
    {
        AnimalScore.text ="Skor Quiz :  " + PlayerPrefs.GetString("AnimalScore");
        HumanScore.text = "HUMAN QUIZ - " + PlayerPrefs.GetString("HumanScore");
        FruitScore.text = "FRUIT QUIZ - " + PlayerPrefs.GetString("FruitScore");
    }

    
}
