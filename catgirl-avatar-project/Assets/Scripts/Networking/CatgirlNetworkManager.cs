// Assets/Scripts/Networking/CatgirlNetworkManager.cs
// 🌸 BambiSleep™ Church Multiplayer Networking System 🌸
// Unity Netcode for GameObjects integration with pink frilly multiplayer

using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobby;
using Unity.Services.Lobby.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BambiSleep.CatGirl.Networking
{
    public class CatgirlNetworkManager : MonoBehaviour
    {
        [Header("🌸 Network Configuration")]
        [SerializeField] private int maxPlayers = 16;
        [SerializeField] private string gameName = "BambiSleep™ CatGirl Universe";

        [Header("🎮 Lobby Settings")]
        [SerializeField] private bool isPrivate = false;
        [SerializeField] private string lobbyName = "Pink Frilly Paradise";

        [Header("💎 Relay Configuration")]
        [SerializeField] private string relayJoinCode = "";

        private NetworkManager networkManager;
        private Lobby currentLobby;
        private Allocation relayAllocation;

        public bool IsHost => networkManager != null && networkManager.IsHost;
        public bool IsClient => networkManager != null && networkManager.IsClient;
        public bool IsConnected => networkManager != null && networkManager.IsConnectedClient;
        public string JoinCode => relayJoinCode;

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();

            if (networkManager == null)
            {
                Debug.LogError("❌ NetworkManager component not found!");
                return;
            }

            // Register callbacks
            networkManager.OnClientConnectedCallback += OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnected;
            networkManager.OnServerStarted += OnServerStarted;
        }

        private void OnDestroy()
        {
            if (networkManager != null)
            {
                networkManager.OnClientConnectedCallback -= OnClientConnected;
                networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
                networkManager.OnServerStarted -= OnServerStarted;
            }

            // Clean up lobby
            if (currentLobby != null)
            {
                LeaveLobby();
            }
        }

        // Host a new multiplayer session
        public async Task<bool> StartHost()
        {
            try
            {
                Debug.Log("🌸 Starting host session...");

                // Create Relay allocation
                relayAllocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

                // Get join code
                relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(relayAllocation.AllocationId);

                Debug.Log($"🔗 Relay Join Code: {relayJoinCode}");

                // Configure Unity Transport with Relay
                networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>()
                    .SetRelayServerData(new Unity.Services.Relay.Models.RelayServerData(relayAllocation, "dtls"));

                // Create lobby
                await CreateLobby();

                // Start as host
                bool started = networkManager.StartHost();

                if (started)
                {
                    Debug.Log("✨ Host started successfully!");
                    return true;
                }
                else
                {
                    Debug.LogError("❌ Failed to start host");
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error starting host: {e.Message}");
                return false;
            }
        }

        // Join an existing session
        public async Task<bool> JoinGame(string joinCode)
        {
            try
            {
                Debug.Log($"🌸 Joining game with code: {joinCode}...");

                relayJoinCode = joinCode;

                // Join Relay allocation
                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                // Configure Unity Transport with Relay
                networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>()
                    .SetRelayServerData(new Unity.Services.Relay.Models.RelayServerData(joinAllocation, "dtls"));

                // Start as client
                bool started = networkManager.StartClient();

                if (started)
                {
                    Debug.Log("✨ Successfully joined game!");
                    return true;
                }
                else
                {
                    Debug.LogError("❌ Failed to join game");
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error joining game: {e.Message}");
                return false;
            }
        }

        // Create a lobby in Unity Lobby service
        private async Task<bool> CreateLobby()
        {
            try
            {
                var options = new CreateLobbyOptions
                {
                    IsPrivate = isPrivate,
                    Data = new Dictionary<string, DataObject>
                    {
                        { "JoinCode", new DataObject(DataObject.VisibilityOptions.Public, relayJoinCode) },
                        { "GameType", new DataObject(DataObject.VisibilityOptions.Public, "CatGirl") }
                    }
                };

                currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

                Debug.Log($"🏛️ Lobby created: {currentLobby.Name} ({currentLobby.Id})");

                // Start heartbeat to keep lobby alive
                StartLobbyHeartbeat();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to create lobby: {e.Message}");
                return false;
            }
        }

        // Find available lobbies
        public async Task<List<Lobby>> FindLobbies()
        {
            try
            {
                var options = new QueryLobbiesOptions
                {
                    Count = 25,
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    }
                };

                var response = await LobbyService.Instance.QueryLobbiesAsync(options);

                Debug.Log($"🔍 Found {response.Results.Count} lobbies");
                return response.Results;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error finding lobbies: {e.Message}");
                return new List<Lobby>();
            }
        }

        // Join an existing lobby
        public async Task<bool> JoinLobby(string lobbyId)
        {
            try
            {
                currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

                // Get join code from lobby data
                if (currentLobby.Data.TryGetValue("JoinCode", out var joinCodeData))
                {
                    await JoinGame(joinCodeData.Value);
                    return true;
                }

                Debug.LogError("❌ No join code found in lobby");
                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error joining lobby: {e.Message}");
                return false;
            }
        }

        // Leave the current lobby
        public async void LeaveLobby()
        {
            if (currentLobby == null) return;

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, Unity.Services.Authentication.AuthenticationService.Instance.PlayerId);
                currentLobby = null;

                Debug.Log("👋 Left lobby");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error leaving lobby: {e.Message}");
            }
        }

        // Heartbeat to keep lobby alive
        private async void StartLobbyHeartbeat()
        {
            while (currentLobby != null && IsHost)
            {
                await Task.Delay(15000); // 15 seconds

                try
                {
                    await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"❌ Lobby heartbeat failed: {e.Message}");
                    break;
                }
            }
        }

        // Disconnect from network
        public void Disconnect()
        {
            if (networkManager == null) return;

            if (IsHost)
            {
                networkManager.Shutdown();
                Debug.Log("🛑 Host session stopped");
            }
            else if (IsClient)
            {
                networkManager.Shutdown();
                Debug.Log("🛑 Disconnected from server");
            }

            LeaveLobby();
        }

        // Network callbacks
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"🌸 Client {clientId} connected!");

            if (IsHost)
            {
                // Host can send welcome message or sync state
                Debug.Log($"👋 Welcome player {clientId} to the pink frilly universe!");
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"👋 Client {clientId} disconnected");
        }

        private void OnServerStarted()
        {
            Debug.Log("✨ Server started successfully!");
        }

        // Get connected players count
        public int GetConnectedPlayersCount()
        {
            if (networkManager == null || !IsConnected) return 0;
            return (int)networkManager.ConnectedClientsIds.Count;
        }

        // Get player list
        public List<ulong> GetConnectedPlayers()
        {
            if (networkManager == null || !IsConnected) return new List<ulong>();
            return new List<ulong>(networkManager.ConnectedClientsIds);
        }
    }
}
