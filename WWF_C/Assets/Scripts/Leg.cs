using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[System.Serializable]
public class Leg {
    public Vector3 idleFootPositionForward;  
    public Vector3 idleFootPositionBack;
    private CharacterLS character;
    private LegController legController;
    [SerializeField] private Transform tStart, t2, tMid, t3, tEnd;
    public Enums.Side side;
    public Transform tFootTarget;
    public Transform tFoot;
    public bool isStepping;
    public bool blockStep;
    private BezierPath bPath;
    private VertexPath vPath;
    //private Vector3 pStart, p2, p3, pEnd;
    private Vector3 pStartRelative; // Start point relatice to player;
    private Vector3 p2Relative; // Point 2 relatice to player;
    [SerializeField] private Transform tStepPathContainer;

    public void Initialize(CharacterLS character) {
        this.character = character;
        legController = character.legController;
    }

    public void MoveFootToIdlePosition(Transform tEnd) {
        character.StartCoroutine(FootToIdleCorutine(tEnd));
        character.legController.StepStarted(side);
    }

    public void StartStep(StepPath stepPath, Vector3 stepDir) {
        stepDir = character.tTargetPitch.localRotation * stepDir;

        Vector3 pStart = tFoot.position;
        pStartRelative = VectorUtils.FromToVector(character.tMain.position, pStart);
        Vector3 p2 = pStart + (stepDir * stepPath.lift.x) + (Vector3.up * stepPath.lift.y);
        p2Relative = VectorUtils.FromToVector(character.tMain.position, p2);

        // Update forward foot
        if (character.telemetry.xzVelocityLocal.z > 0)
            legController.forwardFootSide = side;
        else
            legController.forwardFootSide = side == Enums.Side.left ? Enums.Side.right : Enums.Side.left;

        // Set midpoint
        Vector3 pMid = character.tMain.position + (stepDir * stepPath.mid.x);
        RaycastHit midHit;
        if (Physics.Raycast(pMid, Vector3.down, out midHit, 1, character.layerMask))
            pMid = midHit.point + Vector3.up * stepPath.mid.y;
        else
            pMid += Vector3.down * 1 + Vector3.up * stepPath.mid.y;

        // Set endpoint
        Vector3 pEnd = character.tMain.position + (stepDir * stepPath.end.x);
        RaycastHit endHit;       
        if (Physics.Raycast(pEnd, Vector3.down, out endHit, 1, character.layerMask))
            pEnd = endHit.point;
        else
            pEnd += Vector3.down * 1;

        Vector3 p3 = pEnd + (stepDir * stepPath.extend.x) + (Vector3.up * stepPath.extend.y);

        tStart.position = pStart;
        t2.position = p2;
        tMid.position = pMid;
        t3.position = p3;
        tEnd.position = pEnd;

        character.StartCoroutine(StepCorutine(stepPath, stepDir));
        character.legController.StepStarted(side);
    }

    // public void StartStep(StepPath stepPath, Vector3 stepDir) {
    //     stepDir = character.tTargetPitch.localRotation * stepDir;

    //     Vector3 pStart = tFoot.position;
    //     pStartRelative = VectorUtils.FromToVector(character.body.pelvis.target.position, pStart);
    //     Vector3 p2 = pStart + (stepDir * stepPath.lift.x) + (Vector3.up * stepPath.lift.y);
    //     p2Relative = VectorUtils.FromToVector(character.body.pelvis.target.position, p2);

    //     // Update forward foot
    //     if (character.telemetry.xzVelocityLocal.z > 0)
    //         legController.forwardFootSide = side;
    //     else
    //         legController.forwardFootSide = side == Enums.Side.left ? Enums.Side.right : Enums.Side.left;

    //     // Set midpoint
    //     Vector3 pMid = character.body.pelvis.target.position + (stepDir * stepPath.mid.x);
    //     RaycastHit midHit;
    //     if (Physics.Raycast(pMid, Vector3.down, out midHit, 1, character.layerMask))
    //         pMid = midHit.point + Vector3.up * stepPath.mid.y;
    //     else
    //         pMid += Vector3.down * 1 + Vector3.up * stepPath.mid.y;

    //     // Set endpoint
    //     Vector3 pEnd = character.body.pelvis.target.position + (stepDir * stepPath.end.x);
    //     RaycastHit endHit;       
    //     if (Physics.Raycast(pEnd, Vector3.down, out endHit, 1, character.layerMask))
    //         pEnd = endHit.point;
    //     else
    //         pEnd += Vector3.down * 1;

    //     Vector3 p3 = pEnd + (stepDir * stepPath.extend.x) + (Vector3.up * stepPath.extend.y);

    //     tStepPathContainer.localRotation = Quaternion.identity;
    //     tStart.position = pStart;
    //     t2.position = p2;
    //     tMid.position = pMid;
    //     t3.position = p3;
    //     tEnd.position = pEnd;
    //     tStepPathContainer.localRotation = Quaternion.Euler(70, 0, 0);

    //     character.StartCoroutine(StepCorutine(stepPath, stepDir));
    //     character.legController.StepStarted(side);
    // }

    private IEnumerator FootToIdleCorutine(Transform tEnd) {
        isStepping = true;
        blockStep = true;
        float t = 0;

        Vector3 startPoint = tFootTarget.position;

        while (t < 1) {

            // Create bezier and vertex path
            List<Vector3> points = new List<Vector3>() { startPoint, Vector3.Lerp(startPoint, tEnd.position, 0.5f) + Vector3.up * 0.2f, tEnd.position};
            bPath = new BezierPath(points);
            vPath = new VertexPath(bPath, legController.tPathContainer);

            float stepSpeed = 4; 
            t += Time.deltaTime * stepSpeed;

            // Check if step is far along to allow new step on other leg
            if (t > 1)
                blockStep = false;
            
            // Check if step is finished
            if (t >= 1) {
                t = 0.995f;
                tFootTarget.position = vPath.GetPointAtTime(t);
                break;
            }

            legController.stepT = t;
            tFootTarget.position = vPath.GetPointAtTime(t); // Evalueate foot position
            yield return new WaitForEndOfFrame();
        }

        isStepping = false;
        blockStep = false;       
        yield return null;
    }

    private IEnumerator StepCorutine(StepPath stepPath, Vector3 stepDir) {
        isStepping = true;
        blockStep = true;
        float t = 0;
        while (t < 1) {

            // Offset to left or right depending on what foot (Without this feet will be put in a straight line)
            Vector3 offset = Vector3.Cross(character.telemetry.xzVelocity.normalized, Vector3.up) * legController.footPosOffset;
            if (side == Enums.Side.right)
                offset = -offset;

            // Move the first 2 points with character
            tStart.position = character.tMain.position + pStartRelative;
            t2.position = character.tMain.position + p2Relative;

            // Adjust end position with ground height.
            Vector3 pEnd = tEnd.position + offset;
            RaycastHit hit;
            if (Physics.Raycast(pEnd + Vector3.up, Vector3.down, out hit, 2, character.layerMask))
                pEnd = hit.point;// + Vector3.down;
            else
                pEnd += Vector3.down * 1;

            // The adjusted height diffrence, this will be added to point 3
            float dY = pEnd.y - tEnd.position.y;

            // Create bezier and vertex path
            List<Vector3> points = new List<Vector3>() { tStart.position, t2.position + offset, tMid.position + offset, t3.position + offset + Vector3.up * dY, pEnd};
            bPath = new BezierPath(points);
            vPath = new VertexPath(bPath, legController.tPathContainer);

            float stepSpeed = stepPath.stepSpeed * character.telemetry.xzVelocity.magnitude; 
            t += legController.EvaluateFootMoveSpeed(t) * Time.deltaTime * stepPath.stepSpeed;

            // Body bounce
            if (legController.lastStepSide == side) {
                legController.SetBounce(t);
            }
            
            // Check if step is far along to allow new step on other leg
            if (t > stepPath.blockStepTime)
                blockStep = false;
            
            // Check if step is finished
            if (t >= 1) {
                t = 0.995f;
                tFootTarget.position = vPath.GetPointAtTime(t);
                break;
            }

            legController.stepT = t;
            tFootTarget.position = vPath.GetPointAtTime(t); // Evalueate foot position
            yield return new WaitForEndOfFrame();
        }
        isStepping = false;
        blockStep = false;
        yield return null;
    }
    // private IEnumerator StepCorutine(StepPath stepPath, Vector3 stepDir) {
    //     isStepping = true;
    //     blockStep = true;
    //     float t = 0;
    //     while (t < 1) {

    //         // Offset to left or right depending on what foot (Without this feet will be put in a straight line)
    //         Vector3 offset = Vector3.Cross(character.telemetry.xzVelocity.normalized, Vector3.up) * legController.footPosOffset;
    //         if (side == Enums.Side.right)
    //             offset = -offset;

    //         // Move the first 2 points with character
    //         tStart.position = character.body.pelvis.target.position + pStartRelative;
    //         t2.position = character.body.pelvis.target.position + p2Relative;

    //         // Adjust end position with ground height.
    //         Vector3 pEnd = tEnd.position + offset;
    //         RaycastHit hit;
    //         if (Physics.Raycast(pEnd + Vector3.up, Vector3.down, out hit, 2, character.layerMask))
    //             pEnd = hit.point;// + Vector3.down;
    //         else
    //             pEnd += Vector3.down * 1;

    //         // The adjusted height diffrence, this will be added to point 3
    //         float dY = pEnd.y - tEnd.position.y;

    //         // Create bezier and vertex path
    //         List<Vector3> points = new List<Vector3>() { tStart.position, t2.position + offset, tMid.position + offset, t3.position + offset + Vector3.up * dY, pEnd};
    //         bPath = new BezierPath(points);
    //         vPath = new VertexPath(bPath, legController.tPathContainer);

    //         float stepSpeed = stepPath.stepSpeed * character.telemetry.xzVelocity.magnitude; 
    //         t += legController.EvaluateFootMoveSpeed(t) * Time.deltaTime * stepPath.stepSpeed;

    //         // Body bounce
    //         if (legController.lastStepSide == side) {
    //             legController.SetBounce(t);
    //         }
            
    //         // Check if step is far along to allow new step on other leg
    //         if (t > stepPath.blockStepTime)
    //             blockStep = false;
            
    //         // Check if step is finished
    //         if (t >= 1) {
    //             t = 0.995f;
    //             tFootTarget.position = vPath.GetPointAtTime(t);
    //             break;
    //         }

    //         legController.stepT = t;
    //         tFootTarget.position = vPath.GetPointAtTime(t); // Evalueate foot position
    //         yield return new WaitForEndOfFrame();
    //     }
    //     isStepping = false;
    //     blockStep = false;
    //     yield return null;
    // }

    private void EvalueFootSpeed() {
    }
}