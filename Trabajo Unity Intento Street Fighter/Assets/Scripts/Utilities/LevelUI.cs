using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    // texto 
    public Text AnnouncerTextLine1;
    public Text AnnouncerTextLine2;

    // el timpo que dura la ronda
    public Text LevelTimer;

    // la barra de vida
    public Slider[] healthSliders;

    // marcas de victoria
    public GameObject[] winIndicatorGrids;
    public GameObject winIndicator;

    public static LevelUI instance;
    public static LevelUI GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    // añade el indicador de victoria al canvas de uno de los players
    public void AddWinIndicator(int player)
    {
        GameObject go = Instantiate(winIndicator, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winIndicatorGrids[player].transform);
    }
}
