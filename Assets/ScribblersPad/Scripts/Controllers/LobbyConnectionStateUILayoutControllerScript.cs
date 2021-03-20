using ScribblersSharp;
using UnityEngine;
using UnityEngine.Events;
using UnitySceneLoaderManager;
using UnityTiming.Data;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a lobby connection state UI layout controller script
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class LobbyConnectionStateUILayoutControllerScript : AScribblersClientControllerScript, ILobbyConnectionStateUILayoutController
    {
        /// <summary>
        /// Lobby connection state hash
        /// </summary>
        private static readonly int lobbyConnectionStateHash = Animator.StringToHash("lobbyConnectionState");

        /// <summary>
        /// Timeout timing
        /// </summary>
        [SerializeField]
        private TimingData timeoutTiming = new TimingData(3.0f);

        /// <summary>
        /// Gets invoked when a client starts to connect to a lobby
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbyConnectionStarted = default;

        /// <summary>
        /// Gets invoked when a client connects with a lobby successfully
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbyConnected = default;

        /// <summary>
        /// Gets invoked when a client gets disconnected from its lobby
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbyDisconnected = default;

        /// <summary>
        /// Gets invoked when client gets kicked out from its lobby
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbyKicked = default;

        /// <summary>
        /// Lobby connection state
        /// </summary>
        private ELobbyConnectionState lobbyConnectionState = ELobbyConnectionState.Connecting;

        /// <summary>
        /// Timeout timing
        /// </summary>
        public TimingData TimeoutTiming
        {
            get => timeoutTiming;
            set => timeoutTiming = value;
        }

        /// <summary>
        /// Lobby connection state
        /// </summary>
        public ELobbyConnectionState LobbyConnectionState
        {
            get => lobbyConnectionState;
            private set
            {
                if ((lobbyConnectionState != value) && (lobbyConnectionState != ELobbyConnectionState.Disconnected) && (lobbyConnectionState != ELobbyConnectionState.Kicked))
                {
                    lobbyConnectionState = value;
                    switch (lobbyConnectionState)
                    {
                        case ELobbyConnectionState.Connected:
                            if (onLobbyConnected != null)
                            {
                                onLobbyConnected.Invoke();
                            }
                            OnLobbyConnected?.Invoke();
                            break;
                        case ELobbyConnectionState.Disconnected:
                            if (onLobbyDisconnected != null)
                            {
                                onLobbyDisconnected.Invoke();
                            }
                            OnLobbyDisconnected?.Invoke();
                            break;
                        case ELobbyConnectionState.Kicked:
                            if (onLobbyKicked != null)
                            {
                                onLobbyKicked.Invoke();
                            }
                            OnLobbyKicked?.Invoke();
                            break;
                    }
                    if (!Animator && TryGetComponent(out Animator animator))
                    {
                        Animator = animator;
                    }
                    if (Animator)
                    {
                        Animator.SetInteger(lobbyConnectionStateHash, (int)lobbyConnectionState);
                    }
                }
            }
        }

        /// <summary>
        /// Animator
        /// </summary>
        public Animator Animator { get; private set; }

        /// <summary>
        /// Gets invoked when a client starts to connect to a lobby
        /// </summary>
        public event LobbyConnectionStartedDelegate OnLobbyConnectionStarted;

        /// <summary>
        /// Gets invoked when a client connects with a lobby successfully
        /// </summary>
        public event LobbyConnectedDelegate OnLobbyConnected;

        /// <summary>
        /// Gets invoked when a client gets disconnected from its lobby
        /// </summary>
        public event LobbyDisconnectedDelegate OnLobbyDisconnected;

        /// <summary>
        /// Gets invoked when client gets kicked out from its lobby
        /// </summary>
        public event LobbyKickedDelegate OnLobbyKicked;

        /// <summary>
        /// Gets invoked when "kick-vote" game message has been received
        /// </summary>
        /// <param name="player">Player to be kicked</param>
        /// <param name="voteCount">Kick vote count</param>
        /// <param name="requiredVoteCount">Require kick vote count</param>
        private void ScribblersClientManagerKickVoteGameMessageReceivedEvent(IPlayer player, uint voteCount, uint requiredVoteCount)
        {
            if ((ScribblersClientManager.MyPlayer.ID == player.ID) && (voteCount >= requiredVoteCount))
            {
                LobbyConnectionState = ELobbyConnectionState.Kicked;
            }
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnKickVoteGameMessageReceived += ScribblersClientManagerKickVoteGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnKickVoteGameMessageReceived -= ScribblersClientManagerKickVoteGameMessageReceivedEvent;
        }

        /// <summary>
        /// Shows main menu
        /// </summary>
        public void ShowMainMenu() => SceneLoaderManager.LoadScene("MainMenuScene");

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out Animator animator))
            {
                Animator = animator;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(UnityEngine.Animator) }\" componewnt to this game object.", this);
            }
            if (onLobbyConnectionStarted != null)
            {
                onLobbyConnectionStarted.Invoke();
            }
            OnLobbyConnectionStarted?.Invoke();
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (ScribblersClientManager)
            {
                switch (ScribblersClientManager.WebSocketState)
                {
                    case System.Net.WebSockets.WebSocketState.None:
                    case System.Net.WebSockets.WebSocketState.Connecting:
                        LobbyConnectionState = ELobbyConnectionState.Connecting;
                        break;
                    case System.Net.WebSockets.WebSocketState.Open:
                        LobbyConnectionState = ELobbyConnectionState.Connected;
                        break;
                    case System.Net.WebSockets.WebSocketState.CloseSent:
                    case System.Net.WebSockets.WebSocketState.CloseReceived:
                    case System.Net.WebSockets.WebSocketState.Closed:
                    case System.Net.WebSockets.WebSocketState.Aborted:
                        LobbyConnectionState = ELobbyConnectionState.Disconnected;
                        break;
                }
            }
            if ((lobbyConnectionState == ELobbyConnectionState.Connecting) && (timeoutTiming.ProceedUpdate(false, false) > 0))
            {
                LobbyConnectionState = ELobbyConnectionState.Disconnected;
            }
        }
    }
}
