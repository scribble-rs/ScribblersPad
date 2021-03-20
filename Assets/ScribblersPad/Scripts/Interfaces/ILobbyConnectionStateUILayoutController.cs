using UnityEngine;
using UnityTiming.Data;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a lobby connection state UI layout controller
    /// </summary>
    public interface ILobbyConnectionStateUILayoutController : IScribblersClientController
    {
        /// <summary>
        /// Timeout timing
        /// </summary>
        TimingData TimeoutTiming { get; set; }

        /// <summary>
        /// Lobby connection state
        /// </summary>
        ELobbyConnectionState LobbyConnectionState { get; }

        /// <summary>
        /// Animator
        /// </summary>
        Animator Animator { get; }

        /// <summary>
        /// Gets invoked when a client starts to connect to a lobby
        /// </summary>
        event LobbyConnectionStartedDelegate OnLobbyConnectionStarted;

        /// <summary>
        /// Gets invoked when a client connects with a lobby successfully
        /// </summary>
        event LobbyConnectedDelegate OnLobbyConnected;

        /// <summary>
        /// Gets invoked when a client gets disconnected from its lobby
        /// </summary>
        event LobbyDisconnectedDelegate OnLobbyDisconnected;

        /// <summary>
        /// Gets invoked when client gets kicked out from its lobby
        /// </summary>
        event LobbyKickedDelegate OnLobbyKicked;

        /// <summary>
        /// Shows main menu
        /// </summary>
        void ShowMainMenu();
    }
}
