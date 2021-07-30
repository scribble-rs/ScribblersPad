using ScribblersSharp;
using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a word hint text controller
    /// </summary>
    public interface IWordHintTextController : IScribblersClientController
    {
        /// <summary>
        /// Default word color
        /// </summary>
        UnityEngine.Color DefaultWordColor { get; set; }

        /// <summary>
        /// Correctly guessed word color
        /// </summary>
        UnityEngine.Color CorrectlyGuessedWordColor { get; set; }

        /// <summary>
        /// Word text
        /// </summary>
        TextMeshProUGUI WordText { get; }

        /// <summary>
        /// Gets invoked when my player has guessed correctly
        /// </summary>
        event CorrectlyGuessedDelegate OnCorrectlyGuessed;

        /// <summary>
        /// "ready" game message received event
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// "next-turn" game message received event
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// "update-wordhint" game message received event
        /// </summary>
        event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// "correct-guess" game message received event
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;
    }
}
