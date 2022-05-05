using System;
using System.Net;
using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine;


[RequireComponent(typeof(UnityClient))]
public class ClientManagerL : MonoBehaviour {
    public static ClientManagerL i;

    [Header("Settings")]
    [SerializeField] private string ipAdress;
    [SerializeField] private int port;

    public PlayerManagerL playerManager;
    public MessageManagerL messageManager;
    public LoginManagerL loginManager;
    public GameManagerL gameManager;
    public EquipmentManagerL equipmentManager;
    public ProjectileManagerL projectileManager;
    public SoundManager soundManager;

    public delegate void OnConnectedDelegate();
    public event OnConnectedDelegate OnConnected;

    public UnityClient localClient { get; private set; }
    public ClientConnectionL clientConnection;

    private void Awake() {
        if (i != null) {
            Destroy(gameObject);
            return;
        }
        i = this;

        GameManagerL.gameManagerLLoadedEvent += GameManagerL_gameManagerLLoadedEvent;
        loginManager = new LoginManagerL();
        playerManager.Initialize();
        equipmentManager.Initialize();
        projectileManager.Initialize();
        soundManager.Initialize();

        DontDestroyOnLoad(this);
    }

    private void Start() {
        localClient = GetComponent<UnityClient>();
        clientConnection = new ClientConnectionL(localClient);
        localClient.ConnectInBackground(IPAddress.Parse(ipAdress), port, IPVersion.IPv4, ConnectCallback);
    }

    private void ConnectCallback(Exception exception) {
        if (localClient.ConnectionState == ConnectionState.Connected) {
            OnConnected?.Invoke();
            messageManager = new MessageManagerL();
        }
        else {
            Debug.LogError("Unable to connect to server.");
        }
    }

    private void GameManagerL_gameManagerLLoadedEvent(GameManagerL gameManagerL) {
        this.gameManager = gameManagerL;
    }
}