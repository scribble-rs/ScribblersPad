using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a debug main menu controller
    /// </summary>
    public interface IDebugMainMenuController : IBehaviour
    {
        /// <summary>
        /// Host input field
        /// </summary>
        TMP_InputField HostInputField { get; set; }

        /// <summary>
        /// Lobby ID input field
        /// </summary>
        TMP_InputField LobbyIDInputField { get; set; }

        /// <summary>
        /// Username input field
        /// </summary>
        TMP_InputField UsernameInputField { get; set; }

        /// <summary>
        /// Saves input
        /// </summary>
        void SaveInput();
    }
}
