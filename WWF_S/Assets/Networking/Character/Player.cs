using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum PlayerState { unspawned, spawned };
    public DrDatas.Player.PlayerData playerData;
    public bool isInitialized = false;
    public event Delegates.EmptyDelegate characterSpawnedEvent;
    public GameObject goCharacter;
    [HideInInspector] public Character character; 

    public virtual void Initialize(DrDatas.Player.PlayerData playerData) {
        this.playerData = playerData;
        isInitialized = true;
    }

    public virtual void SpawnCharacter(DrDatas.Player.CharacterData characterData) {    
        goCharacter.name = "Character";
        goCharacter.transform.position = characterData.position;
        goCharacter.transform.rotation = characterData.rotation;
        playerData.state = PlayerState.spawned;
        playerData.characterData = characterData;

        characterSpawnedEvent?.Invoke();
    }
}
