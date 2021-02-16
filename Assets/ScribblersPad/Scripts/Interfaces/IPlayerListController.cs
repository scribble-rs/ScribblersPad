using ScribblersSharp;

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
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

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
