using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a chat input controller
    /// </summary>
    public interface IChatInputController : IScribblersClientController
    {
        /// <summary>
        /// Chat input field
        /// </summary>
        TMP_InputField ChatInputField { get; set; }

        /// <summary>
        /// Submits input
        /// </summary>
        void SubmitInput();
    }
}
