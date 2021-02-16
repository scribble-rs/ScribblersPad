using ScribblersSharp;
using TMPro;
using UnityEngine.UI;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a time view controller
    /// </summary>
    public interface ITimeViewController : IScribblersClientController
    {
        /// <summary>
        /// Current time in seconds string format
        /// </summary>
        string CurrentTimeInSecondsStringFormat { get; set; }

        /// <summary>
        /// Current time in seconds string format string translation
        /// </summary>
        StringTranslationObjectScript CurrentTimeInSecondsStringFormatStringTranslation { get; set; }

        /// <summary>
        /// Current time text
        /// </summary>
        TextMeshProUGUI CurrentTimeText { get; set; }

        /// <summary>
        /// Time progress image
        /// </summary>
        Image TimeProgressImage { get; set; }

        /// <summary>
        /// Current time
        /// </summary>
        float CurrentTime { get; }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;
    }
}
