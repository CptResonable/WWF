using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CharacterLS : Character {
    public Transform tTargetYaw;
    public Transform tTargetPitch;
    public Transform tMain;
    public Transform tCamera;
    public Rigidbody rbMain;
    public PlayerInput input;
    public AnimalController animalController;
    public Locomotion locomotion;
    public Telemetry telemetry;
    public LegController legController;
    public Body body;
    public Torso torso;
    new public EquipmentLS equipment;

    protected override void Awake() {
        equipment.Initialize(this);
        base.Awake();
        base.equipment = equipment;

        body.Initialize();
        //CharacterColliderSetup.SetupIgnores(body);
        telemetry = new Telemetry(this);
        locomotion.Initialize(this);
        legController.Initialize(this);
        torso.Initialize(this);
    }
}
