using UnityEngine;
using System.Collections;

[System.Serializable]
public class ButtonRef : MonoBehaviour {
    //para seleccionar el boton
    public GameObject selectIndicator;

    public bool selected;

    void Start()
    {
        selectIndicator.SetActive(false);
    }
    //se utilizan para seleccionar el boton
    void Update()
    {
        selectIndicator.SetActive(selected);
    }
}
