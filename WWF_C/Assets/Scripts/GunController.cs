using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController {
    private CharacterLS character;
    public Gun gun;
    public State state = State.neutral;

    public GunController(CharacterLS character) {
        this.character = character;

        character.input.attack_1.keyDownEvent += Attack_1_keyDownEvent;
        character.input.attack_1.keyUpEvent += Attack_1_keyUpEvent;
        character.input.reload.keyDownEvent += Reload_keyDownEvent;

        character.locomotion.sprintChangedEvent += Locomotion_sprintChangedEvent;
    }

    #region Input
    private void Attack_1_keyDownEvent() {
        throw new System.NotImplementedException();
    }
    private void Attack_1_keyUpEvent() {
        throw new System.NotImplementedException();
    }
    private void Reload_keyDownEvent() {
        throw new System.NotImplementedException();
    }
    #endregion

    private void Locomotion_sprintChangedEvent(bool isSprinting) {
        //if (isSprinting)
    }

    public virtual void GunEquiped(Gun gun) {
    }

    public virtual void GunUnEquiped(Gun gun) {
    }

    protected void SwitchState(State newState) {
        switch (newState) {
            case State.neutral:
                break;
            case State.reloading:
                break;
        }
    }

    protected virtual void Fire() {
    }

    protected virtual void EnterAds() {
    }

    protected virtual void ExitAds() {
    }

    public virtual void StartReload() {
    }

    protected virtual void CancelReload() {
    }

    protected virtual void ReloadFinished() { 
    }

    public enum State { neutral, reloading}
}
