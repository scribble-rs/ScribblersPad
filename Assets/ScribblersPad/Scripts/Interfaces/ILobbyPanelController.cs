using ScribblersSharp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        /// Unknown language sprite
        /// </summary>
        Sprite UnknownLanguageSprite { get; set; }

        /// <summary>
        /// Is votekicking disabled sprite
        /// </summary>
        Sprite IsVotekickingDisabledSprite { get; set; }

        /// <summary>
        /// Is votekicking enabled sprite
        /// </summary>
        Sprite IsVotekickingEnabledSprite { get; set; }

        /// <summary>
        /// Is not using custom words sprite
        /// </summary>
        Sprite IsNotUsingCustomWordsSprite { get; set; }

        /// <summary>
        /// Is using custom words sprite
        /// </summary>
        Sprite IsUsingCustomWordsSprite { get; set; }

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
        /// Language image
        /// </summary>
        Image LanguageImage { get; set; }

        /// <summary>
        /// Is votekicking enabled image
        /// </summary>
        Image IsVotekickingEnabledImage { get; set; }

        /// <summary>
        /// Is using custom words image
        /// </summary>
        Image OsUsingCustomWordsImage { get; set; }

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
