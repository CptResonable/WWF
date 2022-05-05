public static class Tags {
    public enum Categories { connection = 1, login = 2, player = 3, game = 4, equipment = 5, weapons = 6, health = 7}

    // Client connection
    private const ushort connection = 1000;
    public const ushort connection_clientConnected = connection + 0;
    public const ushort connection_clientDisconnected = connection + 1;

    // Login
    private const ushort login = 2000;
    public const ushort login_loginRequestToServer = login + 0;
    public const ushort login_loginFailed = login + 1;
    public const ushort login_loginSuccess = login + 2;

    // Player
    private const ushort player = 3000;
    public const ushort player_createPlayer = player + 0;
    public const ushort player_createAllPlayers = player + 1;
    public const ushort player_requestSpawn = player + 2;
    public const ushort player_spawnPlayer = player + 3;
    public const ushort player_updateInput = player + 4;
    public const ushort player_playerDisconnected = player + 5;

    // Game
    private const ushort game = 4000;
    public const ushort game_updateGame = game + 0;

    // Equipment
    private const ushort equipment = 5000;
    public const ushort equipment_equipItem = equipment + 0;

    // Weapons
    private const ushort weapons = 6000;
    public const ushort weapons_gunFired = weapons + 0;
    public const ushort weapons_gunFiredUnverified = weapons + 1;
    public const ushort weapons_gunFiredVerified = weapons + 2;

    // Health 
    private const ushort health = 7000;
    public const ushort health_damageTaken = health + 0;

        // // Projectiles
    // private const ushort projectiles = 6000;
    // public const ushort projectiles_requestProjectileLaunch = projectiles + 0;
    // public const ushort projectiles_projectileLaunched = projectiles + 1;
}
