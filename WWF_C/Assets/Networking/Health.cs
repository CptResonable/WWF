using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Health {
    public enum State { alive, knocked, dead}
    public State state;
    public float maxHP = 100;
    public float HP = 100;

    [SerializeField] private DamageReceiver[] damageReceivers;
    private Character character;

    public delegate void HpSetStaticDelegate(Character character, float change, float newHP); // Used for static events
    public delegate void HpSetDelegate(float change, float newHP); // Used for non static events
    public static event HpSetStaticDelegate hpSetStaticEvent;
    public event HpSetDelegate hpSetEvent;
    public delegate void StateChangedDelegate(Character character, State state);
    public static event StateChangedDelegate stateChangedEvent;

    public void Initialize(Character character) {
        this.character = character;

        // Subscribe to damage received events from all damage receivers
        for (int i = 0; i < damageReceivers.Length; i++) {
            damageReceivers[i].damageReceivedEvent += DamageReceiver_damageReceivedEvent;
        }

        character.updateEvent += Update;
    }

    private void Update() {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
            SetState(State.knocked);
    }
    
    public void SetHP(float newHP) {
        float change = newHP - HP;
        HP = newHP; 

        hpSetStaticEvent?.Invoke(character, change, HP);
        hpSetEvent?.Invoke(change, HP);
    }

    // Listens to damage events, only on server for now
    private void DamageReceiver_damageReceivedEvent(float damage) {
        SetHP(HP - damage);
        
        if (HP <= 0) {
            if (state == State.alive)
                SetState(State.knocked);
        }
    }

    public void SetState(State newState) {
        switch (newState) {
            case State.alive:
                break;
            case State.knocked:
                GetKnocked();
                break;
            case State.dead:
                Die();
                break;
        }

        stateChangedEvent?.Invoke(character, state);
    }

    private void GetKnocked() {
        state = State.knocked;

        // Debug.Log(character.name + " knocked");
        // if (character is CharacterLS) {
        //     CharacterLS c = character as CharacterLS;
        //     c.rbMain.GetComponent<PhysicallyCopyRotation>().strengthMod = 0;
        //     c.body.SetAllStrengthMods(0);
        //     c.locomotion.hooverStrengthMod = 0;
        //     c.locomotion.movementStrengthMod = 0;
        // }
    }

    private void Die() {
        state = State.dead;    
    }

    // private void SetState(State newState) {
    //     state = newState;
    //     DrDatas.HealthData.HealthStateChangedData stateChangedData = new DrDatas.HealthData.HealthStateChangedData(character.GetClientID(), state);
    // }
}
