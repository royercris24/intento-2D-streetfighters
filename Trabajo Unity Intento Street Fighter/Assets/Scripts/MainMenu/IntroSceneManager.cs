using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{

    public GameObject startText;
    float timer;
    bool loadingLevel;
    bool init;

    public int activeElement;
    public GameObject menuObj;
    public ButtonRef[] menuOptions;

    // para desabilitar el objeto principal
    void Start()
    {
        menuObj.SetActive(false);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!init)
        {   
            // para quitar el texto "Press Space" 
            timer += Time.deltaTime;
            if (timer > 0.6f)
            {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy);
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
            {
                init = true;
                startText.SetActive(false);
                // quita el texto y deja utilizar el menu
                menuObj.SetActive(true); 
            }
        }
        else
        {
            if (!loadingLevel)
            {
                // indica la opcion que has elegido
                menuOptions[activeElement].selected = true;

                // eliges las obciones con las flechas
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    menuOptions[activeElement].selected = false;

                    if (activeElement > 0)
                    {
                        activeElement--;
                    }
                    else
                    {
                        activeElement = menuOptions.Length - 1;
                    }
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    menuOptions[activeElement].selected = true;

                    if (activeElement < menuOptions.Length - 1)
                    {
                        activeElement++;
                    }
                    else
                    {
                        activeElement = 0;
                    }
                }

                // para activar la seleccion que has elegido con las flechas 
                // y nos llevara a la escena level
                if (Input.GetKey(KeyCode.Space) || Input.GetButtonUp("Jump"))
                {
                    Debug.Log("load");
                    loadingLevel = true;
                    StartCoroutine("LoadLevel");
                    menuOptions[activeElement].transform.localScale *= 1.2f;
                }
            }
        }
    }

    
    // para saber cuantos jugadores estaran en el level manager
    void HandSelectedOption()
    {
        switch (activeElement)
        {
            case 0:
                CharacterManager.GetInstace().numberOfUser = 1;
                break;
            case 1:
                CharacterManager.GetInstace().numberOfUser = 2;
                CharacterManager.GetInstace().players[1].playerType = PlayerBase.PlayerType.user;
                break;
        }
    }

    // lo que hacer es cargar la siguiente escena al seleccionar la cantidad de los jugadores
    IEnumerator LoadLevel()
    {
        HandSelectedOption();
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync("level", LoadSceneMode.Single);
    }
}
