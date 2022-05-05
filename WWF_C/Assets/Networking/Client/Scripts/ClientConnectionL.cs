using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class ClientConnectionL {
    public static ClientConnectionL i;
    public UnityClient client { get; }
    public enum State { loggedOut, loggedIn };
    public State state { get; private set; }

    public ClientConnectionL(UnityClient client) {
        i = this;

        state = State.loggedOut;
        this.client = client;
    }
}
