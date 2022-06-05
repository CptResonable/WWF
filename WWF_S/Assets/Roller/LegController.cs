using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using VacuumBreather;

[System.Serializable]
public class LegController {
    private CharacterLS character;
    public enum LegState { standing, walking, running}
    public LegState legState;
    public Leg legL, legR;
    public AnimationCurve speedCurve; // Speed scaler along the step
    public float stepT;
    public float moveSpeed;
    public IdleTarget idleTarget;
    public event Delegates.EmptyDelegate stepStartedEvent;
    [HideInInspector] public Enums.Side lastStepSide;
    [HideInInspector] public Enums.Side forwardFootSide;

    public float hooverHeight = 0.7f;
    [SerializeField] public AnimationCurve velocityToHooverHeight;

    [Header("Object references")]
    [SerializeField] public Transform tPathContainer;

    [Header("Step constants")]
    [SerializeField] private float startWalkSpeed; // 
    [SerializeField] private float startRunSpeed; // Speed at wich player will start to transition to from the walk stepPath to the run stepPath
    [SerializeField] private float maximumRunSpeed;
    [SerializeField] private float walkStepStartRatio;
    [SerializeField] private float strafeFootPosOffset;
    [SerializeField] private float forwardFootPosOffset;
    [HideInInspector] public float footPosOffset;

    [Header("Pelvis step rotation")]
    [SerializeField] private AnimationCurve strafePelvisRotationVelocityCurve;
    [SerializeField] private float strafePelvisRotationAmount;
    [SerializeField] private float pelvisRotationAmount;

    [Header("Step paths")]
    [SerializeField] private StepPath toIdlePath;
    [SerializeField] private StepPath slowWalkPath;
    [SerializeField] private StepPath walkPath;
    [SerializeField] private StepPath runPath;

    [Header("Bounce")]
    [SerializeField] private Vector3 bouncePeakOffset;
    [SerializeField] private float bounceLerpSpeed;
    [SerializeField] public AnimationCurve bounceCurve;
    [SerializeField] private AnimationCurve bounceVelocityCurve;
    [SerializeField] private float bounceHeight;
    private float bounceYTarget;
    private float bounceY;
    private Vector3 bounceV3Target;
    private Vector3 bounceV3;


    public void Initialize(CharacterLS character) {
        this.character = character;
        tPathContainer = character.GetPlayer().transform;

        legL.Initialize(character);
        legR.Initialize(character);

        character.updateEvent += Update;
        character.lateUpdateEvent += LateUpdate;
    }

    private void Update() {   
        moveSpeed = character.telemetry.xzVelocity.magnitude;
        hooverHeight = velocityToHooverHeight.Evaluate(moveSpeed);

        StepPath stepPath;
        if (moveSpeed < startWalkSpeed) {
            stepPath = slowWalkPath;
        }
        else if (moveSpeed < startRunSpeed) {
            float t = Mathf.InverseLerp(startWalkSpeed, startRunSpeed, moveSpeed);
            stepPath = InterpolateStepPaths(slowWalkPath, walkPath, t);
        }
        else if (moveSpeed < maximumRunSpeed) {
            float t = Mathf.InverseLerp(startRunSpeed, maximumRunSpeed, moveSpeed);
            stepPath = InterpolateStepPaths(walkPath, runPath, t);
        }
        else {
            stepPath = runPath;
        }

        Vector3 pelvisToFootL = VectorUtils.RemoveYComponent(VectorUtils.FromToVector(character.body.pelvis.target.position, legL.tFootTarget.position));
        Vector3 pelvisToFootR = VectorUtils.RemoveYComponent(VectorUtils.FromToVector(character.body.pelvis.target.position, legR.tFootTarget.position));

        Vector3 moveDirection = character.telemetry.xzVelocity.normalized;
        float lDst = Vector3.Dot(moveDirection, pelvisToFootL);
        float rDst = Vector3.Dot(moveDirection, pelvisToFootR);

        // Initiate step (maybe)
        if (!legR.blockStep && !legL.blockStep) {
            if (character.telemetry.xzVelocity.magnitude > 0.2f) { // Make steps if player is walking/running 
                if (lDst < rDst) {
                    if (lDst < 0) {
                        if (rDst < 0 || (Mathf.Abs(lDst) > rDst * walkStepStartRatio))
                            legL.StartStep(stepPath, moveDirection.normalized);
                    }
                }
                else if (rDst < lDst) {
                    if (rDst < 0) {
                        if (lDst < 0 || (Mathf.Abs(rDst) > lDst * walkStepStartRatio))
                            legR.StartStep(stepPath, moveDirection.normalized);
                    }
                }
            }
            else { // Move feet to idle position if player is not walking/running 
                float l = VectorUtils.FromToVector(legL.tFootTarget.position, idleTarget.tIdleTargetL.position).magnitude;
                float r = VectorUtils.FromToVector(legR.tFootTarget.position, idleTarget.tIdleTargetR.position).magnitude;
                if (l > r) {
                    if (l > 0.1f)
                        legL.MoveFootToIdlePosition(idleTarget.tIdleTargetL);
                }
                else {
                    if (r > 0.1f)
                        legR.MoveFootToIdlePosition(idleTarget.tIdleTargetR);
                }
            }
        }

        float strafeAmountThing = Vector3.Dot(character.tTargetYaw.right, character.telemetry.xzVelocity.normalized); // This thing 1 when going right, -1 when going left and 0 when going forward or backwards

        // Set foot offset thing
        if (Vector3.Dot(character.telemetry.xzVelocity.normalized, character.tTargetYaw.forward) > 0)
            footPosOffset = Mathf.Lerp(forwardFootPosOffset, strafeFootPosOffset, Mathf.Abs(strafeAmountThing));
        else
            footPosOffset = Mathf.Lerp(-forwardFootPosOffset, strafeFootPosOffset, Mathf.Abs(strafeAmountThing));

        PelvisOffset();
        PelvisRotation(strafeAmountThing);

        idleTarget.SetIdleTarget(forwardFootSide, character);
    }

    private void LateUpdate() {
        character.body.pelvis.target.rotation *= Quaternion.Euler(0, -pelvisAngle, 0);
    }

    public void StepStarted(Enums.Side side) {
        lastStepSide = side;
        stepStartedEvent?.Invoke();
    }

    public float EvaluateFootMoveSpeed(float t) {
        float speed;
        if (moveSpeed < startWalkSpeed) {
            speed = slowWalkPath.footSpeedCurve.Evaluate(t);
        }
        else if (moveSpeed < startRunSpeed) {
            float t2 = Mathf.InverseLerp(startWalkSpeed, startRunSpeed, t);
            speed = Mathf.Lerp(slowWalkPath.footSpeedCurve.Evaluate(t), walkPath.footSpeedCurve.Evaluate(t), t2);
        }
        else if (moveSpeed < maximumRunSpeed) {
            float t2 = Mathf.InverseLerp(startRunSpeed, maximumRunSpeed, t);
            speed = Mathf.Lerp(walkPath.footSpeedCurve.Evaluate(t), runPath.footSpeedCurve.Evaluate(t), t2);
        }
        else {
            speed = runPath.footSpeedCurve.Evaluate(t);
        }

        return speed;
    }


    public void SetBounce(float t) {
        float curveValue;

        if (moveSpeed < startWalkSpeed) {
            curveValue = slowWalkPath.bounceCurve.Evaluate(t);
        }
        else if (moveSpeed < startRunSpeed) {
            float t2 = Mathf.InverseLerp(startWalkSpeed, startRunSpeed, t);
            curveValue = Mathf.Lerp(slowWalkPath.bounceCurve.Evaluate(t), walkPath.bounceCurve.Evaluate(t), t2);
        }
        else if (moveSpeed < maximumRunSpeed) {
            float t2 = Mathf.InverseLerp(startRunSpeed, maximumRunSpeed, t);
            curveValue = Mathf.Lerp(walkPath.bounceCurve.Evaluate(t), runPath.bounceCurve.Evaluate(t), t2);
        }
        else {
            curveValue = runPath.bounceCurve.Evaluate(t);
        }

        curveValue = bounceCurve.Evaluate(t);
        bounceV3Target = curveValue * bounceVelocityCurve.Evaluate(character.telemetry.xzVelocity.magnitude) * bouncePeakOffset;
    }

    //public void SetBounce(float t) {
    //    float curveValue;

    //    if (moveSpeed < startWalkSpeed) {
    //        curveValue = slowWalkPath.bounceCurve.Evaluate(t);
    //    }
    //    else if (moveSpeed < startRunSpeed) {
    //        float t2 = Mathf.InverseLerp(startWalkSpeed, startRunSpeed, t);
    //        curveValue = Mathf.Lerp(slowWalkPath.bounceCurve.Evaluate(t), walkPath.bounceCurve.Evaluate(t), t2);
    //    }
    //    else if (moveSpeed < maximumRunSpeed) {
    //        float t2 = Mathf.InverseLerp(startRunSpeed, maximumRunSpeed, t);
    //        curveValue = Mathf.Lerp(walkPath.bounceCurve.Evaluate(t), runPath.bounceCurve.Evaluate(t), t2);
    //    }
    //    else {
    //        curveValue = runPath.bounceCurve.Evaluate(t);
    //    }
    //    bounceYTarget = curveValue * bounceVelocityCurve.Evaluate(character.telemetry.xzVelocity.magnitude) * bounceHeight;
    //}

    //private void PelvisOffset() {
    //    bounceY = Mathf.Lerp(bounceY, bounceYTarget, Time.deltaTime * bounceLerpSpeed);
    //    character.torso.positionOffset = new Vector3(0, -bounceY, 0);
    //}

    private void PelvisOffset() {
        bounceV3 = Vector3.Lerp(bounceV3, bounceV3Target, Time.deltaTime * bounceLerpSpeed);
        character.torso.positionOffset = bounceV3;
    }

    private float pelvisAngle;
    // Make pelvis rotate when walk, because that is how people walk irl
    private void PelvisRotation(float strafeAmountThing) {

        // Forward backward rotation
        Vector3 dFoot = VectorUtils.RemoveYComponent(VectorUtils.FromToVector(legL.tFoot, legR.tFoot));
        float forwardBackRotation = -Vector3.Dot(character.tTargetYaw.forward, dFoot) * pelvisRotationAmount;

        // Strafe rotaion
        float strafeRotation = (strafeAmountThing > 0) ? strafePelvisRotationAmount : -strafePelvisRotationAmount;
        strafeRotation *= strafePelvisRotationVelocityCurve.Evaluate(character.telemetry.xzVelocity.magnitude);
        
        // Lerp between forward and strafe rotation
        pelvisAngle = Mathf.Lerp(forwardBackRotation, strafeRotation, Mathf.Abs(strafeAmountThing));

        // Apply roation
        character.body.pelvis.ikTarget.localRotation = Quaternion.Euler(180, pelvisAngle, 0);
    }

    private StepPath InterpolateStepPaths(StepPath pathA, StepPath pathB, float t) {
        return new StepPath(
            Vector2.Lerp(pathA.lift, pathB.lift, t),
            Vector2.Lerp(pathA.mid, pathB.mid, t),
            Vector2.Lerp(pathA.extend, pathB.extend, t),
            Vector2.Lerp(pathA.end, pathB.end, t),
            Mathf.Lerp(pathA.blockStepTime, pathB.blockStepTime, t),
            Mathf.Lerp(pathA.stepSpeed, pathB.stepSpeed, t),
            AnimationCurveHelpers.InterpolateCurve(pathA.bounceCurve, pathB.bounceCurve, t)
        );
    }
}

[System.Serializable]
public struct StepPath {
    public Vector2 lift; // A point after the foot is lifted (Along player velocity, relative to foot start point)
    public Vector2 mid; // Foot highpoint;
    public Vector2 extend; // Where the foot is at the point whete leg is the most extended (Along player velocity, relative to player)
    public Vector2 end; // Where the foot ends up (Along player velocity, relative to player)
    public float blockStepTime; // In how much of the step is other leg step blocked?
    public float stepSpeed; // The speed of the step, will be multiplied with character velocity
    public AnimationCurve bounceCurve;
    public AnimationCurve footSpeedCurve;

    public StepPath(Vector2 lift, Vector2 mid, Vector2 extend, Vector2 end, float blockStepTime, float stepSpeed, AnimationCurve bounceCurve) {
        this.lift = lift;
        this.mid = mid;
        this.extend = extend;
        this.end = end;
        this.blockStepTime = blockStepTime;
        this.stepSpeed = stepSpeed;
        this.bounceCurve = bounceCurve;
        footSpeedCurve = new AnimationCurve();
    }
}

[System.Serializable]
public struct IdleTarget {
    public Transform tIdleTargetL;
    public Transform tIdleTargetR;
    public Vector3 leftForwardLocal;
    public Vector3 rightForwardLocal;
    public Vector3 leftBackwardLocal;
    public Vector3 rightBackwardLocal;

    public void SetIdleTarget(Enums.Side forwardSide, Character character) {
        if (forwardSide == Enums.Side.left) {
            tIdleTargetL.localPosition = leftForwardLocal;
            tIdleTargetR.localPosition = rightBackwardLocal;
        }
        else {
            tIdleTargetL.localPosition = leftBackwardLocal;
            tIdleTargetR.localPosition = rightForwardLocal;
        }

        RaycastHit hitL;
        if (Physics.Raycast(tIdleTargetL.position + Vector3.up, Vector3.down, out hitL, 2, character.layerMask))
            tIdleTargetL.position = hitL.point;
        else
            tIdleTargetL.position += Vector3.down * 1;
        
        RaycastHit hitR;
        if (Physics.Raycast(tIdleTargetR.position + Vector3.up, Vector3.down, out hitR, 2, character.layerMask))
            tIdleTargetR.position = hitR.point;
        else
            tIdleTargetR.position += Vector3.down * 1;

        GizmoManager.i.DrawSphere(Time.deltaTime, Color.cyan, tIdleTargetL.position, 0.2f);
        GizmoManager.i.DrawSphere(Time.deltaTime, Color.magenta, tIdleTargetR.position, 0.2f);
    }
}