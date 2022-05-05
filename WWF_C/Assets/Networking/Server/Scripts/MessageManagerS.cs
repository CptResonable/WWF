using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManagerS : MonoBehaviour {
    public static MessageManagerS i;

    public MessageManagerS_connection MM_connection;

    private void Awake() {
        MM_connection = new MessageManagerS_connection();
        i = this;
    }
}
