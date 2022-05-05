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
    public LeanController LeanController;
    public Telemetry telemetry;
    public Head head;
    public LegController legController;
    public Body body;
    public Torso torso;
    new public EquipmentLS equipment;

    protected override void Awake() {
        Debug.Log("fdsf AWAKE_0");
        equipment.Initialize(this);
        base.Awake();
        base.equipment = equipment;

        body.Initialize();
        //CharacterColliderSetup.SetupIgnores(body);
        telemetry = new Telemetry(this);
        locomotion.Initialize(this);
        LeanController.Initialize(this);
        legController.Initialize(this);
        head.Initialize(this);
        torso.Initialize(this);
    }
}
