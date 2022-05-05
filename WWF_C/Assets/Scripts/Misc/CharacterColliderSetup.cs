using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterColliderSetup {
    public static void SetupIgnores(Body body) {
        // Physics.IgnoreCollision(body.arm_2_L.ragdoll.GetComponent<Collider>(), body.arm_2_R.ragdoll.GetComponent<Collider>());
        // Physics.IgnoreCollision(body.hand_L.ragdoll.GetComponent<Collider>(), body.hand_R.ragdoll.GetComponent<Collider>());

        Debug.Log(body.leg_1_L.ragdoll.name);   
        Physics.IgnoreCollision(body.leg_1_L.ragdoll.GetComponent<Collider>(), body.leg_1_R.ragdoll.GetComponent<Collider>());
        Physics.IgnoreCollision(body.leg_1_L.ragdoll.GetComponent<Collider>(), body.leg_2_R.ragdoll.GetComponent<Collider>());
        Physics.IgnoreCollision(body.leg_2_L.ragdoll.GetComponent<Collider>(), body.leg_2_R.ragdoll.GetComponent<Collider>());
        Physics.IgnoreCollision(body.leg_2_L.ragdoll.GetComponent<Collider>(), body.leg_1_R.ragdoll.GetComponent<Collider>());
    }
}
