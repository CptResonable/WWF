using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using UnityEngine;

public class ServerManagerS : MonoBehaviour {
    public static ServerManagerS i;

    private XmlUnityServer xmlServer;
    public DarkRiftServer server;
    public GameManagerS gameManager;
    public EquipmentManagerS equipmentManager;
    public LoginManagerS loginManager;
    public PlayerManagerS playerManager;
    public ProjectileManagerS projectileManager;

    public Dictionary<ushort, ClientConnectionS> clientConnections = new Dictionary<ushort, ClientConnectionS>();

    public delegate void MsgRecieved(object sender, MessageReceivedEventArgs e);
    public event MsgRecieved msgRecieved;

    private void Awake() {
        if (i != null) {
            Destroy(gameObject);
            return;
        }
        i = this;
        DontDestroyOnLoad(this);

        playerManager.Initialize();
        equipmentManager.Initialize();
        projectileManager.Initialize();

        SceneSwapper.i.LoadScene("SampleScene");
    }

    private void Start() {
        xmlServer = GetComponent<XmlUnityServer>();
        server = xmlServer.Server;
        server.ClientManager.ClientConnected += OnClientConnected;
        server.ClientManager.ClientDisconnected += OnClientDisconnected;
    }

    private void OnDestroy() {
        server.ClientManager.ClientConnected -= OnClientConnected;
        server.ClientManager.ClientDisconnected -= OnClientDisconnected;
    }
    public void GameSceneLoaded(GameManagerS gameManager) {
        this.gameManager = gameManager;
    }

    private void OnClientConnected(object sender, ClientConnectedEventArgs e) {
        e.Client.MessageReceived += Client_MessageReceived;

        clientConnections.Add(e.Client.ID, new ClientConnectionS(e.Client));
    }

    private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
        IClient client = e.Client;
        playerManager.DisconnectPlayer(e.Client.ID); // Disconnect player

        ClientConnectionS p;
        if (clientConnections.TryGetValue(client.ID, out p)) {
            //p.OnClientDisconnect(sender, e);
        }
        e.Client.MessageReceived -= Client_MessageReceived;
    }

    private void Client_MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage()) {

            // Extract tag cagegory from tag
            Tags.Categories tagCategory = (Tags.Categories)Mathf.FloorToInt((float)message.Tag / 1000.0f);
            Debug.Log("MSG IN");

            // Send message to be handeled at the right place
            switch (tagCategory) {
                case Tags.Categories.connection:
                    break;
                case Tags.Categories.login:
                    loginManager.MessageRecieved(message, e);
                    break;
                case Tags.Categories.player:
                    playerManager.MessageRecieved(message, e);
                    Debug.Log("MSG IN PLAYER");
                    break;
                case Tags.Categories.weapons:
                    equipmentManager.weaponManager.MessageRecieved(message, e);
                    Debug.Log("?????");
                    break;
                default:
                    break;
            }
        }
    }
}
