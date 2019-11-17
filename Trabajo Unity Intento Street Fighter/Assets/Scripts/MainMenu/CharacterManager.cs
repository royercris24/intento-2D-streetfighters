using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public int numberOfUser;
    public List<PlayerBase> players = new List<PlayerBase>(); // lista de todos los jugadores

    //el id y el correspondiente prefab del character
    public List<CharacterBase> characterList = new List<CharacterBase>();

    
    // sirve para buscar el el character desde su id
    public CharacterBase returnCharacterWithID(string id)
    {
        CharacterBase retVal = null;

        for (int i = 0; i < characterList.Count; i++)
        {
            if (string.Equals(characterList[i].charId,id))
            {
                retVal = characterList[i];
            }
        }
        return retVal;
    }

    
    // "para devolverle al jugador su character "
    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase retVal = null;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerStates == states)
            {
                retVal = players[i];
                break;
            }
        }
        return retVal;
    }

    
    internal int numberOfPlayer;
    
    // para aceder decualquier sitio
    public static CharacterManager instance;
    public static CharacterManager GetInstace()
    {
        return instance;
    }

    // esto era para no destruir los personajes que se seleccionaban en otra escena
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


}

[System.Serializable]
public class CharacterBase
{
    //el nombre que le das al prefab
    public string charId;
    // el prefab mismo
    public GameObject prefab;
}


[System.Serializable]
public class PlayerBase
{
    // caracteristicas del jugador
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    public enum PlayerType
    {
        user, // humano
        ai, // maquina
    }
}

