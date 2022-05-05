using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

[System.Serializable]
public class Locomotion {
    private CharacterLS character;

    [SerializeField] private Vector3 pidValues_move;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 pidValues_hoover;
    [SerializeField] private float targetHooverHeight;
    [SerializeField] private Transform tPitch;
    [SerializeField] private Transform tOffset;
    [SerializeField] private float accelerationLeanAmount;
    [SerializeField] private float lateralForceLimit;

    private Vector3 targetMoveVector;

    public void Initialize(CharacterLS character) {
        this.character = character;

        character.updateEvent += Update;
        character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    private void Update() {
        if (UnityEngine.InputSystem.Keyboard.current.upArrowKey.wasPressedThisFrame)
            moveSpeed += 1;

        if (UnityEngine.InputSystem.Keyboard.current.downArrowKey.wasPressedThisFrame)
            moveSpeed -= 1;
    }

    private void Character_fixedUpdateEvent() {
        Hoover();
        Move();
    }

    private void Hoover() {
        float hooverHeight = 10;
        RaycastHit hit_down;

        float tHeight =  targetHooverHeight;

        if (Physics.Raycast(character.rbMain.position, Vector3.down, out hit_down, 10, character.layerMask)) {
            hooverHeight = hit_down.distance;
        }

        PidController pid_y = new PidController(pidValues_hoover.x, pidValues_hoover.y, pidValues_hoover.z);
        float error = tHeight - hooverHeight;
        float delta = -character.rbMain.velocity.y;
        float output = pid_y.ComputeOutput(error, delta, Time.deltaTime);

        if (hooverHeight < tHeight + 0.1f) {
            Vector3 normalForce = character.telemetry.groundNormal * output;
            Vector3 normalUpForce = new Vector3(0, normalForce.y, 0);
            Vector3 normalXZForce = normalForce - normalUpForce;
            if (character.input.vecMoveXZ.magnitude < 0.1f && normalXZForce.magnitude < lateralForceLimit)
                normalXZForce = Vector3.zero;
            character.rbMain.AddForce(normalUpForce + normalXZForce);
        }
    }

    private void Move() {

        // Inititate PIDs.
        PidController pid_x = new PidController(pidValues_move.x, pidValues_move.y, pidValues_move.z);
        PidController pid_z = new PidController(pidValues_move.x, pidValues_move.y, pidValues_move.z);

        Vector3 xzForward = new Vector3(character.tTargetYaw.forward.x, 0, character.tTargetYaw.forward.z).normalized;
        Vector3 xzRight = new Vector3(character.tTargetYaw.right.x, 0, character.tTargetYaw.right.z).normalized;
        Vector3 vecMove = (character.rbMain.transform.forward * character.input.vecMoveXZ.y + character.rbMain.transform.transform.right * character.input.vecMoveXZ.x) * moveSpeed * Time.deltaTime;

        // I use velocity to target velocity for PID error.
        Vector3 error = vecMove * moveSpeed - character.rbMain.velocity;

        // Do PID calculations to get move forces.
        Vector3 moveForce = Vector3.zero;
        moveForce.x = pid_x.ComputeOutput(error.x, character.telemetry.acceleration.x, Time.deltaTime);
        moveForce.z = pid_z.ComputeOutput(error.z, character.telemetry.acceleration.z, Time.deltaTime);

        character.rbMain.AddForce(moveForce);
    }

    private void FrictionForce() {
        targetMoveVector = character.rbMain.transform.TransformVector(character.input.v3vecMoveXZ);

        float angle = Vector3.Angle(Vector3.up, character.telemetry.groundNormal);
        float N = 9.81f * character.rbMain.mass * Mathf.Cos(angle * Mathf.Deg2Rad);
        float F = 0.95f * N;

        Vector3 force = -character.rbMain.velocity.normalized * F;
        if (Vector3.Angle(targetMoveVector, force) > 90) {
            force -= Vector3.Project(force, targetMoveVector.normalized);
        }

        character.rbMain.AddForce(force);
    }
}