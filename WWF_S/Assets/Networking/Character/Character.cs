using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
    protected Player player;
    public Health health;
    public LayerMask layerMask;
    public Equipment equipment;
    public DrDatas.Player.PlayerBodyData bodyData; // Add thus to character data
    
    public event Delegates.EmptyDelegate updateEvent;
    public event Delegates.EmptyDelegate fixedUpdateEvent;
    public event Delegates.EmptyDelegate lateUpdateEvent;

    protected virtual void Awake() {
        Debug.Log("AWAKE_0");
        player = transform.parent.GetComponent<Player>();
        Debug.Log("AWAKE_1");
        equipment.Initialize(this);
        Debug.Log("AWAKE_2");
        health.Initialize(this);
    }

    protected virtual void Update() {
        updateEvent?.Invoke();
    }

    protected virtual void FixedUpdate() {
        fixedUpdateEvent?.Invoke();
    }

    protected virtual void LateUpdate() {
        lateUpdateEvent?.Invoke();
    }

    public Player GetPlayer() {
        Debug.Log("Getting player!");
        if (player == null) {
            Debug.Log("Player is null!");
            player = transform.parent.GetComponent<Player>();
        }
        Debug.Log("Player: " + player.name);

        return player;
    }

    public abstract ushort GetClientID();
}
