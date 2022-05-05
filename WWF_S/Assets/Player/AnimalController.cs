using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalController {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float targetHooverHeight;
    [SerializeField] private float recovery;
    [SerializeField] private float fallStopLerper;
    [SerializeField] private AnimationCurve angleToSpeed;

    private CharacterLS character;
    private CharacterController characterController;
    private RaycastHit hit_down;
    private Vector3 moveVector = Vector3.zero;
    private Vector3 velocity;
    private float yVelocity;
    private float heightBelowTarget;
    private float hooverHeight;

    public void Initialize(CharacterLS character) {
        this.character = character;
        characterController = character.tMain.GetComponent<CharacterController>();
        character.updateEvent += Update;
        character.input.jump.keyDownEvent += Input_jump;
    }

    private void Update() {
        Vector3 targetMoveVector = (character.tMain.forward * character.input.vecMoveXZ.y + character.tMain.right * character.input.vecMoveXZ.x) * moveSpeed;
        targetMoveVector = Vector3.ProjectOnPlane(targetMoveVector, hit_down.normal);

        if (Physics.Raycast(character.tMain.position, Vector3.down, out hit_down, 10, character.layerMask)) {
            hooverHeight = hit_down.distance;
            heightBelowTarget = targetHooverHeight - hooverHeight;
        }

        velocity += Vector3.down * 9.81f * Time.deltaTime;

        if (heightBelowTarget > 0) {
            Vector3 adjustedVelocity = Vector3.ProjectOnPlane(velocity, hit_down.normal);
            velocity = Vector3.Lerp(velocity, adjustedVelocity, 5 * Time.deltaTime);
            //velocity = adjustedVelocity;
            characterController.Move(Vector3.up * heightBelowTarget * recovery * Time.deltaTime);
        }

        velocity = Vector3.Lerp(velocity, targetMoveVector, Time.deltaTime * 2);

        characterController.Move(velocity * Time.deltaTime);

        float targetMoveSpeed = moveSpeed;

        // Vector3 targetMoveVector = (character.tMain.forward * character.input.vecMoveXZ.y + character.tMain.right * character.input.vecMoveXZ.x);
        // float groundAngle = Vector3.SignedAngle(Vector3.up, hit_down.normal, Vector3.Cross(targetMoveVector, Vector3.up));
        // float speedMod = angleToSpeed.Evaluate(groundAngle);
        // targetMoveVector *= moveSpeed * speedMod;
        // Debug.Log("angle: " + groundAngle);

        // moveVector = Vector3.Lerp(moveVector, targetMoveVector, Time.deltaTime * accelerationSpeed);

        // characterController.Move(moveVector * Time.deltaTime);

        // Hoover();
        // Gravity();

        // characterController.Move(moveVector * Time.deltaTime);
        // characterController.Move(Vector3.up * yVelocity * Time.deltaTime);

        // if (characterController.isGrounded && yVelocity < 0)
        //     yVelocity = 0;
    }

    private void Input_jump() {
        yVelocity += jumpForce;
    }

    // private void Hoover() {
    //     float hooverHeight = 10;

    //     float tHeight = targetHooverHeight;

    //     if (Physics.Raycast(character.tMain.position, Vector3.down, out hit_down, 10, character.layerMask)) {
    //         hooverHeight = hit_down.distance;
    //     }

    //     heightBelowTarget = targetHooverHeight - hooverHeight;

    //     //Vector3 targetMoveVector = (character.tMain.forward * character.input.vecMoveXZ.y + character.tMain.right * character.input.vecMoveXZ.x);
    //     Vector3 v = velocity;
    //     // Retrurn player to hoverheight if character is below it
    //     if (heightBelowTarget > 0) {

    //         v = Vector3.ProjectOnPlane(velocity, hit_down.normal);

    //         // // Lerp yVelocity to zero if it less than that
    //         // if (yVelocity < 0)
    //         //     yVelocity = Mathf.Lerp(yVelocity, 0, fallStopLerper * Time.deltaTime);

    //         // // Move character up
    //         // characterController.Move(Vector3.up * heightBelowTarget * recovery * Time.deltaTime);
    //     }
    // }

    // private void Gravity() {
    //     yVelocity -= 9.81f * Time.deltaTime;
    //     // if (heightBelowTarget < 0)
    //     //     yVelocity -= 9.81f * Time.deltaTime;
    // }
}
