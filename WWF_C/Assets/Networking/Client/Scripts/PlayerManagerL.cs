using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

[System.Serializable]
public class PlayerManagerL {
    public HealthManagerL healthManager;

    [SerializeField] private Transform localPlayerContainer;
    [SerializeField] private Transform networkPlayerContainer;
    
    public Dictionary<ushort, PlayerN> networkPlayers = new Dictionary<ushort, PlayerN>();
    public Dictionary<ushort, Player> allPlayers = new Dictionary<ushort, Player>();
    public PlayerL localPlayer;

    public static event Delegates.EmptyDelegate playerManagerInitializedEvent;
    public delegate void PlayerCreatedDelegate(DrDatas.Player.PlayerData playerData);
    public event PlayerCreatedDelegate playerCreatedEvent;
    public delegate void CharacterSpawnedDelegate(DrDatas.Player.CharacterData characterData);
    public event CharacterSpawnedDelegate characterSpawnedEvent;

    public void Initialize() {
        GameManagerL.gameManagerLLoadedEvent += GameManagerL_gameManagerLLoadedEvent;

        healthManager.Initialize();
    }

    private void GameManagerL_gameManagerLLoadedEvent(GameManagerL gameManagerL) {
        ClientManagerL.i.gameManager.gameUpdateReceivedEvent += GameManager_gameUpdateReceivedEvent;
    }

    private void GameManager_gameUpdateReceivedEvent(DrDatas.Game.GameUpdateData updateData) {
        UpdateBodyDatas(updateData.bodyDatas);
    }

    #region Receive messages
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.player_createPlayer:
                OnMsg_createPlayer(message, e);
                break;
            case Tags.player_createAllPlayers:
                OnMsg_createAllPlayers(message, e);
                break;
            case Tags.player_spawnPlayer:
                OnMsg_spawnCharacter(message, e);
                break;
            case Tags.player_playerDisconnected:
                OnMsg_disconnectPlayer(message, e);
                break;
            default:
                break;
        }
    }

    private void OnMsg_createPlayer(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.PlayerData playerData = message.Deserialize<DrDatas.Player.PlayerData>();

        if (playerData.clientId == ClientManagerL.i.localClient.ID)
            CreateLocalPlayer(playerData);
        else
            CreateNetworkPlayer(playerData);
    }

    private void OnMsg_createAllPlayers(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.AllPlayerDatas playerDatas = message.Deserialize<DrDatas.Player.AllPlayerDatas>();

        for (int i = 0; i < playerDatas.playerDatas.Length; i++) {
            if (playerDatas.playerDatas[i].clientId == ClientManagerL.i.localClient.ID)
                CreateLocalPlayer(playerDatas.playerDatas[i]);
            else
                CreateNetworkPlayer(playerDatas.playerDatas[i]);
        }
    }

    private void OnMsg_spawnCharacter(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.CharacterData characterData = message.Deserialize<DrDatas.Player.CharacterData>();
        SpawnCharacter(characterData);
        // allPlayers[characterData.clientId].SpawnCharacter(characterData);

        // Debug.Log("SPAWN CHARACTER EQUP LENGH: " + characterData.equipmentData.equipables.Length);

        // characterSpawnedEvent?.Invoke(characterData);
    }

    private void OnMsg_disconnectPlayer(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.PlayerDisconnectedData disconnectedData = message.Deserialize<DrDatas.Player.PlayerDisconnectedData>();
        GameObject.Destroy(allPlayers[disconnectedData.clientId].gameObject);
        allPlayers.Remove(disconnectedData.clientId);

        if (disconnectedData.clientId != ClientConnectionL.i.client.ID)
            networkPlayers.Remove(disconnectedData.clientId);
    }
    #endregion

    #region Send messages
    public void SendMsg_requestSpawn(ushort spawnpointId) {
        DrDatas.Player.RequestCharacterSpawnData requestSpawnData = new DrDatas.Player.RequestCharacterSpawnData(spawnpointId);

        using (Message msgOut = Message.Create(Tags.player_requestSpawn, requestSpawnData))
            ClientManagerL.i.localClient.SendMessage(msgOut, SendMode.Reliable);
    }

    public void SendMsg_updateInput(DrDatas.Player.PlayerInputData inputData) {
        using (Message msgOut = Message.Create(Tags.player_updateInput, inputData))
            ClientManagerL.i.localClient.SendMessage(msgOut, SendMode.Reliable);
    }
    #endregion
    
    private void UpdateBodyDatas(DrDatas.Player.PlayerBodyData[] bodyDatas) {
        
        for (int i = 0; i < bodyDatas.Length; i++) {
            if (bodyDatas[i].clientId != ClientConnectionL.i.client.ID) {
                networkPlayers[bodyDatas[i].clientId].character.bodyN.CopyRigFromData(bodyDatas[i]);
            }
        }
    }

    private void CreateLocalPlayer(DrDatas.Player.PlayerData playerData) {
        GameObject goNewPlayer = GameObject.Instantiate(GameObjects.i.playerL, localPlayerContainer);
        goNewPlayer.name = playerData.clientId + " " + playerData.username;
        PlayerL newPlayer = goNewPlayer.GetComponent<PlayerL>();
        newPlayer.Initialize(playerData);
        localPlayer = newPlayer;
        allPlayers.Add(playerData.clientId, newPlayer);
    }

    private void CreateNetworkPlayer(DrDatas.Player.PlayerData playerData) {
        GameObject goNewPlayer = GameObject.Instantiate(GameObjects.i.playerN, networkPlayerContainer);
        goNewPlayer.name = playerData.clientId + " " + playerData.username;
        PlayerN newPlayer = goNewPlayer.GetComponent<PlayerN>();
        newPlayer.Initialize(playerData);
        networkPlayers.Add(playerData.clientId, newPlayer);
        allPlayers.Add(playerData.clientId, newPlayer);

        if (playerData.state == Player.PlayerState.spawned)
            SpawnCharacter(playerData.characterData);

        playerCreatedEvent?.Invoke(playerData);
    }

    private void SpawnCharacter(DrDatas.Player.CharacterData characterData) {
        allPlayers[characterData.clientId].SpawnCharacter(characterData);
        characterSpawnedEvent?.Invoke(characterData);
    }
}
