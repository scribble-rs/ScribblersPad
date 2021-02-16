using ScribblersSharp;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a game UI layout controller
    /// </summary>
    public interface IGameUILayoutController : IScribblersClientController
    {
        /// <summary>
        /// Game UI layout state
        /// </summary>
        EGameUILayoutState GameUILayoutState { get; set; }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when drawing board has been shown
        /// </summary>
        event DrawingBoardShownDelegate OnDrawingBoardShown;

        /// <summary>
        /// Gets invoked when chat has been shown
        /// </summary>
        event ChatShownDelegate OnChatShown;

        /// <summary>
        /// Shows drawing board
        /// </summary>
        void ShowDrawingBoard();

        /// <summary>
        /// Shows chat
        /// </summary>
        void ShowChat();
    }
}
