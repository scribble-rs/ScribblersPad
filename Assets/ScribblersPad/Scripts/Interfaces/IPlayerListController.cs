using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a player list controller
    /// </summary>
    public interface IPlayerListController : IScribblersClientController
    {
        /// <summary>
        /// Player list element asset
        /// </summary>
        GameObject PlayerListElementAsset { get; set; }

        /// <summary>
        /// Own player list element asset
        /// </summary>
        GameObject OwnPlayerListElementAsset { get; set; }

        /// <summary>
        /// Player list element parent rectangle transform
        /// </summary>
        RectTransform PlayerListElementParentRectangleTransform { get; set; }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        event NameChangeGameMessageReceivedDelegate OnNameChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;
    }
}
