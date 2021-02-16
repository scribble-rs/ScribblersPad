using ScribblersSharp;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a start game controller
    /// </summary>
    public interface IStartGameController : IScribblersClientController
    {
        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked before your own game has started
        /// </summary>
        event BeforeOwningGameStartedDelegate OnBeforeOwningGameStarted;

        /// <summary>
        /// Starts game
        /// </summary>
        void StartGame();
    }
}
