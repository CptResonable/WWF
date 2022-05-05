using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class UiHud : MonoBehaviour {
    [SerializeField] private UiHudElement_healthBar healthBar;
    // [SerializeField] private List<UiHudElement> elementsList; // Only used to copy elements into elements dictionary;
    // private Dictionary<UiHudElement.ID, UiHudElement> elements = new Dictionary<UiHudElement.ID, UiHudElement>();

    private void Awake() {
        ClientManagerL.i.playerManager.characterSpawnedEvent += PlayerManager_characterSpawnedEvent;
    }

    private void PlayerManager_characterSpawnedEvent(DrDatas.Player.CharacterData characterData) {
        Character character = ClientManagerL.i.playerManager.allPlayers[characterData.clientId].character;

        if (character.GetClientID() == ClientManagerL.i.localClient.ID) {
            healthBar.Initialize(character.health);
            healthBar.Enable();
        }
    }
}
