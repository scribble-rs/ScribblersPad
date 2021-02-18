using ScribblersSharp;
using TMPro;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a username input field controller
    /// </summary>
    public interface IUsernameInputFieldController : IScribblersClientController
    {
        /// <summary>
        /// Username is too long title string translation
        /// </summary>
        StringTranslationObjectScript UsernameIsTooLongTitleStringTranslation { get; set; }

        /// <summary>
        /// Username is too long message string translation
        /// </summary>
        StringTranslationObjectScript UsernameIsTooLongMessageStringTranslation { get; set; }

        /// <summary>
        /// Username input field
        /// </summary>
        TMP_InputField UsernameInputField { get; }

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
        /// Submits username
        /// </summary>
        void SubmitUsernameChanges();
    }
}
