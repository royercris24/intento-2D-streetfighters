using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public const int PlayerHealth = 150; // la cantidad de vida del character

    WaitForSeconds oneSec; // significa que hay que esperar 1 segundo
    public Transform[] spawnPositions; // donde apareceran los personajes

    CharacterManager charM; 
    LevelUI levelUI; // acceso al ui

    public int maxTurns = 3; // las rondas que se necesitan para ganar
    int currentTurn = 1; // en la ronda que estamos, empezando por 1

    // las variables de countdown
    public bool countdown;
    public int maxTurnTimer = 30;
    int currentTimer;
    float internalTimer;

    // Use this for initialization
    void Start()
    {
        // referencias del singleton
        charM = CharacterManager.GetInstace();
        levelUI = LevelUI.GetInstance();
        
        // esperas un segundo
        oneSec = new WaitForSeconds(1);

        // los anuncios estan cerrados
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //orienta el character en la escena
        if (charM.players[0].playerStates.transform.position.x < charM.players[1].playerStates.transform.position.x)
        {
            charM.players[0].playerStates.lookRight = true;
            charM.players[1].playerStates.lookRight = false;
        }
        else
        {
            charM.players[0].playerStates.lookRight = false;
            charM.players[1].playerStates.lookRight = true;
        }
    }

    void Update()
    {
        if (countdown)
        {
            HandleTurnTimer(); // control del tiempo
        }
    }

    void HandleTurnTimer()
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime; // cada segundo
        if (internalTimer > 1)
        {
            currentTimer--;
            internalTimer = 0;
        }

        if (currentTimer <= 0) // si el tiempo llega a cero
        {
            EndTurnFunction(true); // se termina la ronda
            countdown = false;
        }
    }


    IEnumerator StartGame()
    {
        // cuando empezamos la partida
        // creamos el character
        yield return CreatePlayers();
        // y empezamos el turno
        yield return InitTurn();
    }

    IEnumerator InitTurn()
    {
        // desactivar los anuncios
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        // resetear el tiempo
        currentTimer = maxTurnTimer;
        countdown = false;

        yield return InitPlayers();
        yield return EnableControl();
    }

    // para crear el character
    IEnumerator CreatePlayers()
    {   
        // va a por los jugadores en la lista 
        for (int i = 0; i < charM.players.Count; i++)
        {
            GameObject go = Instantiate(charM.players[i].playerPrefab, spawnPositions[i].position, Quaternion.identity) as GameObject;
            charM.players[i].playerStates = go.GetComponent<StateManager>();
            charM.players[i].playerStates.healthSlider = levelUI.healthSliders[i];
        }
        yield return null;
    }

    IEnumerator InitPlayers()
    {
        for (int i = 0; i < charM.players.Count; i++)
        {
            // resetea la vida cuando empieza una ronda
            charM.players[i].playerStates.health = PlayerHealth;
            charM.players[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.players[i].playerStates.transform.position = spawnPositions[i].position;
        }
        yield return null;
    }

    IEnumerator EnableControl()
    {
        // anuncios que saldran en cada ronda
        levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine1.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine1.color = Color.white;
        yield return oneSec;
        yield return oneSec;

        // segundos que hay que esperar
        levelUI.AnnouncerTextLine1.text = "3";
        levelUI.AnnouncerTextLine1.color = Color.green;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "2";
        levelUI.AnnouncerTextLine1.color = Color.yellow;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "1";
        levelUI.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "FIGHT!";
        levelUI.AnnouncerTextLine1.color = Color.red;

        for (int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                InputHandler ih = charM.players[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.players[i].inputId;
                ih.enabled = true;
            }
        }
        // despues de un segundo se quitara el anuncio
        yield return oneSec;
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;
    }

    // desactiva el control de los characters
    void DisableControl()
    {
        for (int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.ResetStateInputs();

            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                charM.players[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }
        }
    }

    public void EndTurnFunction(bool timeOut = false)
    {
        // lo llamamos cuando queremos terminar la ronda, 
        // pero antes hay que saver si se ha acabado el tiempo
        countdown = false;
        // resetea el tiempo ui
        levelUI.LevelTimer.text = maxTurnTimer.ToString();

        // si se a acabado el tiempo
        if (timeOut)
        {   
            // se añade estos anuncios
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "Time Out!";
            levelUI.AnnouncerTextLine1.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "K.O";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }
        // desactivas los controles
        DisableControl();
        // termina la ronda
        StartCoroutine("EndTurn");
    }

    IEnumerator EndTurn()
    {
        // espera 3 segundos
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        // busca que jugador a ganado
        PlayerBase vPlayer = FindWinningPlayer();

        if (vPlayer == null)
        {
            // empate
            levelUI.AnnouncerTextLine1.text = "Draw";
            levelUI.AnnouncerTextLine1.color = Color.blue;
        }
        else
        {
            // victoria
            levelUI.AnnouncerTextLine1.text = vPlayer.playerId + " WINS!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

        // espera 3 segundos
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;


        currentTurn++;

        bool matchOver = isMatchOver();

        if (!matchOver)
        {
            StartCoroutine("InitTurn"); // empieza el loop para la siguiente otra ronda 
        }
        else
        {
            // cuando uno de los jugadores gane 3 rondas, se volvera a la pantalla inicial
            for (int i = 0; i < charM.players.Count; i++)
            {
                charM.players[i].score = 0;
                charM.players[i].hasCharacter = false;
            }
            SceneManager.LoadSceneAsync("intro");
        }
    }

    bool isMatchOver()
    {
        bool retVal = false;

        for (int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].score >= maxTurns)
            {
                retVal = true;
                break;
            }
        }
        return retVal;
    }

    PlayerBase FindWinningPlayer()
    {
        // para encontrar quien ganoo la ronda
        PlayerBase retVal = null;
        StateManager targetPlayer = null;
        
        // mira si los characters tienen la misma vida
        if (charM.players[0].playerStates.health != charM.players[1].playerStates.health)
        {
            // si no, busca el character con menor vida, entonces el otro character es el ganador
            if (charM.players[0].playerStates.health < charM.players[1].playerStates.health)
            {
                charM.players[1].score++;
                targetPlayer = charM.players[1].playerStates;
                levelUI.AddWinIndicator(1);
            }
            else
            {
                charM.players[0].score++;
                targetPlayer = charM.players[0].playerStates;
                levelUI.AddWinIndicator(0);
            }
            retVal = charM.returnPlayerFromStates(targetPlayer);
        }
        return retVal;
    }

    public static LevelManager instance;
    public static LevelManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
}
