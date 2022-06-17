using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRecieverComponent : MonoBehaviour {
    //public HitReceiverComponent[] hitReceivers;
    public HitReceiverCollection receiverCollection;
    //public HitRecieverObject[] hitReceivers;

    private void Awake() {
        receiverCollection.Initialize(this);
        //for (int i = 0; i < hitReceivers.Length; i++) {
        //    hitReceivers[i].Initialize(this);
        //}
    }

    public void Hit(Vector3 position, Quaternion rotation) {
        receiverCollection.Hit(position, rotation);
        //for (int i = 0; i < hitReceivers.Length; i++) {
        //    hitReceivers[i].Hit(position, rotation);
        //}
    }
}

public abstract class HitRecieverObject : ScriptableObject {
    protected HitRecieverComponent hitReciever;

    public void Initialize(HitRecieverComponent hitReciever) {
        this.hitReciever = hitReciever;
    }

    public abstract void Hit(Vector3 position, Quaternion rotation);
}

//[System.Serializable]
//public abstract class HitReceiverComponent {
//    public abstract void Hit();
//}

//public class HitVfxReceiver : HitReceiverComponent {
//    public override void Hit() { 
//    }
//}

//public class HitSfxReceiver : HitReceiverComponent {
//    public override void Hit() {
//    }
//}
