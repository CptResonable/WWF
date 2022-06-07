using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Telemetry {
    private CharacterLS character;

    public Vector3 velocity;
    public Vector3 xzVelocity;
    public Vector3 xzVelocityLocal;
    public Vector3 acceleration;
    public Vector3 xzAcceleration;
    public Vector3 xzAccelerationLocal;

    public Vector3 xzForward;
    public Vector3 xzRight;
    public float velocityAngle; // Angle between forward direction and velocity direction
    public bool isGoingBackwards;
    public Enums.Side velocitySide;
    public Vector3 groundNormal = Vector3.up;
    public Vector3 COM;
    public Vector3 dFoot;

    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastVelocity = Vector3.zero;

    public Telemetry(CharacterLS character) {
        this.character = character;
        lastPosition = character.tMain.position;

        character.StartCoroutine(DelayedInitialization());
    }

    // This is just a workaround for server character spazzing out on spawn, could not find the reason for that happening
    private IEnumerator DelayedInitialization() {
        yield return new WaitForSeconds(0.1f);
        lastPosition = character.tMain.position;
        character.fixedUpdateEvent += Player_fixedUpdateEvent;
    }

    private void Player_fixedUpdateEvent() {
        velocity = VectorUtils.FromToVector(lastPosition, character.tMain.position) / Time.deltaTime;
        lastPosition = character.tMain.position;
        CalculateCOM();
        //GizmoManager.i.DrawSphere(Time.fixedDeltaTime, Color.yellow, COM, 0.07f);

        xzVelocity = new Vector3(velocity.x, 0, velocity.z);
        xzVelocityLocal = character.tMain.InverseTransformVector(xzVelocity);

        acceleration = Vector3.Lerp(acceleration,(velocity - lastVelocity) / Time.fixedDeltaTime, 0.05f);
        xzAcceleration = new Vector3(acceleration.x, 0, acceleration.z);
        xzAccelerationLocal = character.tMain.InverseTransformVector(xzAcceleration);
        lastVelocity = velocity;

        xzForward = new Vector3(-character.tMain.forward.x, 0, -character.tMain.forward.z);
        xzRight = new Vector3(character.tMain.right.x, 0, character.tMain.right.z);

        GizmoManager.i.DrawLine(Time.deltaTime, Color.blue, character.tMain.position, character.tMain.position + xzAcceleration * 0.5f);
        GizmoManager.i.DrawLine(Time.deltaTime, Color.red, COM, COM + xzRight);

        velocityAngle = Vector3.SignedAngle(xzForward, xzVelocity, Vector3.up);
        //velocityAngle = Vector3.Angle(xzForward, xzVelocity);
        if (Mathf.Abs(velocityAngle) > 90)
            isGoingBackwards = true;
        else
            isGoingBackwards = false;
        
        if (velocityAngle < 0)
            velocitySide = Enums.Side.left;
        else
            velocitySide = Enums.Side.right;

        // Groun normal
        RaycastHit hit;
        if (Physics.Raycast(character.tMain.position, Vector3.down, out hit, 10, character.layerMask))
            groundNormal = hit.normal;


        dFoot = VectorUtils.FromToVector(character.body.foot_L.ikTarget.position, character.body.foot_R.ikTarget.position);

        // RaycastHit hit;
        // if (Physics.Raycast(character.body.armature.ragdoll.position, Vector3.down, out hit, 10, character.layerMask))
        //     groundNormal = hit.normal;
        //groundNormal
    }

    private void CalculateCOM() {
        Vector3 newCOM = Vector3.zero;
        float totalMass = 0;

        for (int i = 0; i < character.body.rigidbodies.Length; i++) {
            newCOM += character.body.rigidbodies[i].position * character.body.rigidbodies[i].mass;
            totalMass += character.body.rigidbodies[i].mass;
        }

        newCOM /= totalMass;
        //velocity = VectorUtils.FromToVector(COM, newCOM) / Time.fixedDeltaTime;
        //velocity = character.rbMain.velocity;
        COM = newCOM;
    }
}
