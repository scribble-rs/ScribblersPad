using ScribblersSharp;
using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Lobby language dropdown controller interface
    /// </summary>
    public interface ILobbyLanguageDropdownController : IBehaviour
    {
        /// <summary>
        /// Dropdown
        /// </summary>
        TMP_Dropdown Dropdown { get; }

        /// <summary>
        /// Selected lobby language
        /// </summary>
        ELanguage SelectedLanguage { get; set; }
    }
}
