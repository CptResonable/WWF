using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

[System.Serializable]
public class PlayerManagerS {
    public HealthManagerS healthManager;

    [SerializeField] private Transform playerContainer;
    public Dictionary<ushort, PlayerS> players = new Dictionary<ushort, PlayerS>();
    public delegate void PlayerCreatedDelegate(DrDatas.Player.PlayerData playerData);
    public delegate void CharacterSpawnedDelegate(CharacterS character);
    public event PlayerCreatedDelegate playerCreatedEvent;
    public event CharacterSpawnedDelegate characterSpawnedEvent;
    public int characterCount = 0; // Number of players witch characters

    public void Initialize() {
        ServerManagerS.i.loginManager.playerLoggedInEvent += LoginManager_playerLoggedInEvent;

        healthManager.Initialize();
    }

    #region Messages recieved
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.player_requestSpawn:
                OnMsg_spawnRequest(message, e);
                break;
            case Tags.player_updateInput:
                OnMsg_updateInput(message, e);
                break;
            default:
                break;
        }
    }

    public void OnMsg_spawnRequest(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.RequestCharacterSpawnData requestSpawnData = message.Deserialize<DrDatas.Player.RequestCharacterSpawnData>();

        SpawnpointManager.SpawnpointData spawnpointData;
        if (SpawnpointManager.i.GetSpawnpointData(requestSpawnData.spawnpointId, out spawnpointData)) {
            DrDatas.Player.CharacterData characterData = new DrDatas.Player.CharacterData(e.Client.ID, spawnpointData.position, spawnpointData.rotation, new DrDatas.EquipmentDatas.CharacterEquipmentData(new DrDatas.EquipmentDatas.EquipableData[0]));

            //// TODO: Make sure spawn action is allowed
            players[e.Client.ID].SpawnCharacter(characterData);
            characterCount++;

            characterSpawnedEvent?.Invoke(players[e.Client.ID].character);

            // Tell everyone to spawn the character
            using (Message msgOut = Message.Create(Tags.player_spawnPlayer, characterData)) {
                foreach (PlayerS player in ServerManagerS.i.playerManager.players.Values)
                    player.client.SendMessage(msgOut, SendMode.Reliable);
            }
        }
    }

    public void OnMsg_updateInput(Message message, MessageReceivedEventArgs e) {
        DrDatas.Player.PlayerInputData inputData = message.Deserialize<DrDatas.Player.PlayerInputData>();
        Debug.Log("INPUT UPDATE!");
        // Debug.Log("Comented out this");
        players[e.Client.ID].character.input.OnMsg_updateInput(inputData);
    }

    #endregion

    private void LoginManager_playerLoggedInEvent(ushort clientId, DrDatas.Login.LoginData loginData) {
        CreateNewPlayer(clientId, loginData.username);
        
        // Gather all player datas
        DrDatas.Player.PlayerData[] playerDatas = new DrDatas.Player.PlayerData[players.Count];
        int i = 0;
        foreach (PlayerS player in players.Values) {
            playerDatas[i] = player.playerData;
            if (player.client.ID != clientId)
                Debug.Log("TEST 2 EQUIPMENT COUNT: " + playerDatas[i].characterData.equipmentData.equipables.Length);
            i++;
        }

        // Send new player all player datas
        using (Message msgOut = Message.Create(Tags.player_createAllPlayers, new DrDatas.Player.AllPlayerDatas(playerDatas))) {
            players[clientId].client.SendMessage(msgOut, SendMode.Reliable);
        }
    }

    private void CreateNewPlayer(ushort clientId, string username) {
        DrDatas.Player.PlayerData playerData = new DrDatas.Player.PlayerData(clientId, username, Player.PlayerState.unspawned);
        GameObject goNewPlayer = GameObject.Instantiate(GameObjects.i.playerS, playerContainer);
        PlayerS newPlayer = goNewPlayer.GetComponent<PlayerS>();
        newPlayer.Initialize(playerData);

        players.Add(playerData.clientId, newPlayer);

        // Tell all existing players to create new player
        using (Message msgOut = Message.Create(Tags.player_createPlayer, playerData)) {
            foreach (PlayerS player in ServerManagerS.i.playerManager.players.Values)
                if (player != newPlayer)
                    player.client.SendMessage(msgOut, SendMode.Reliable);
        }

        playerCreatedEvent?.Invoke(playerData);
    }

    ///<summary> Disconnect a player from the game </summary>
    public void DisconnectPlayer(ushort clientId) {
        DrDatas.Player.PlayerDisconnectedData disconnectedData = new DrDatas.Player.PlayerDisconnectedData(clientId);
        GameObject.Destroy(players[clientId].gameObject); // Destroy player
        players.Remove(clientId);
        characterCount--;

        // Tell players to destroy dissconnected player
        using (Message msgOut = Message.Create(Tags.player_playerDisconnected, disconnectedData)) {
            foreach (PlayerS player in ServerManagerS.i.playerManager.players.Values) {
                if (player.client != null)
                    player.client.SendMessage(msgOut, SendMode.Reliable);
            }
        }
    }
    
    ///<summary> Get an array of all players body data, used when syncing rig to clients </summary>
    public DrDatas.Player.PlayerBodyData[] GatherBodyDatas() {
        DrDatas.Player.PlayerBodyData[] bodyDatas = new DrDatas.Player.PlayerBodyData[characterCount];
        int i = 0;
        foreach (PlayerS player in players.Values) {

            if (player.character == null)
                continue;
                
            player.character.bodyData.Update(player.character.body);
            bodyDatas[i] = player.character.bodyData;
            i++;
        }
        return bodyDatas;
    }
}
