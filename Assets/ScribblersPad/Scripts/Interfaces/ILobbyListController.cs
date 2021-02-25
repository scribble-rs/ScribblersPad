using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a lobby list controller
    /// </summary>
    public interface ILobbyListController : IBehaviour
    {
        /// <summary>
        /// Lobby panel asset
        /// </summary>
        GameObject LobbyPanelAsset { get; set; }

        /// <summary>
        /// Host input field
        /// </summary>
        TMP_InputField HostInputField { get; set; }

        /// <summary>
        /// Is using secure protocols toggle
        /// </summary>
        Toggle IsUsingSecureProtocolsToggle { get; set; }

        /// <summary>
        /// Gets invoked when listing lobbies has been requested
        /// </summary>
        event ListLobbyRequestedDelegate OnListLobbyRequested;

        /// <summary>
        /// Gets invoked when listing lobbies has failed
        /// </summary>
        event ListLobbyRequestFailedDelegate OnListLobbyRequestFailed;

        /// <summary>
        /// Gets invoked when listing lobbies has been finished
        /// </summary>
        event ListLobbyRequestFinishedDelegate OnListLobbyRequestFinished;

        /// <summary>
        /// Lists lobbies
        /// </summary>
        void ListLobbies();

        /// <summary>
        /// Resets host to default
        /// </summary>
        void ResetHostToDefault();
    }
}
