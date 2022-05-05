using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

public class ClientConnectionS {
    public enum State { loggedOut, loggedIn };
    public State state { get; private set; }
    public IClient client { get; }
    public PlayerS player { get; private set; }

    public ClientConnectionS(IClient client) {
        this.client = client;
    }
}