using ScribblersSharp;
using TMPro;
using UnityEngine;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a lobby panel controller
    /// </summary>
    public interface ILobbyPanelController : IBehaviour
    {
        /// <summary>
        /// Language image animator
        /// </summary>
        Animator LanguageImageAnimator { get; set; }

        /// <summary>
        /// Is votekicking enabled image animator
        /// </summary>
        Animator IsVotekickingEnabledImageAnimator { get; set; }

        /// <summary>
        /// Is using custom words image animator
        /// </summary>
        Animator IsUsingCustomWordsImageAnimator { get; set; }

        /// <summary>
        /// Current round string format
        /// </summary>
        string CurrentRoundStringFormat { get; set; }

        /// <summary>
        /// Current round string format string translation
        /// </summary>
        StringTranslationObjectScript CurrentRoundStringFormatStringTranslation { get; set; }

        /// <summary>
        /// Player count string format
        /// </summary>
        string PlayerCountStringFormat { get; set; }

        /// <summary>
        /// Player count string format string translation
        /// </summary>
        StringTranslationObjectScript PlayerCountStringFormatStringTranslation { get; set; }

        /// <summary>
        /// Current round text
        /// </summary>
        TextMeshProUGUI CurrentRoundText { get; set; }

        /// <summary>
        /// Player count text
        /// </summary>
        TextMeshProUGUI PlayerCountText { get; set; }

        /// <summary>
        /// Lobby view
        /// </summary>
        ILobbyView LobbyView { get; }

        /// <summary>
        /// Sets values for component
        /// </summary>
        /// <param name="lobbyView">Lobby view</param>
        void SetValues(ILobbyView lobbyView);

        /// <summary>
        /// Joins lobby
        /// </summary>
        void JoinLobby();
    }
}
