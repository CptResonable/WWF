using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerL : GunController{
    private CharacterLS character;
    public Gun gun;

    public GunControllerL(CharacterLS character) : base(character) {
        this.character = character;
    }


    public override void GunEquiped(Gun gun) {

    }

    public override void GunUnEquiped(Gun gun) {

    }

    public override void StartReload() {
    }

    protected override void CancelReload() {
    }
    protected override void ReloadFinished() { 
    }
}
