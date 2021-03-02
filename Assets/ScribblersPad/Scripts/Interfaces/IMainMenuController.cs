using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a main menu controller
    /// </summary>
    public interface IMainMenuController : IBehaviour
    {
        /// <summary>
        /// Main menu UI layout state
        /// </summary>
        EMainMenuUILayoutState MainMenuUILayoutState { get; set; }

        /// <summary>
        /// Animator
        /// </summary>
        Animator Animator { get; }

        /// <summary>
        /// Gets invoked when main menu has been shown
        /// </summary>
        event MainMenuShownDelegate OnMainMenuShown;

        /// <summary>
        /// Gets invoked when play menu has been shown
        /// </summary>
        event PlayMenuShownDelegate OnPlayMenuShown;

        /// <summary>
        /// Gets invoked when lobby settings menu has been shown
        /// </summary>
        event LobbySettingsMenuShownDelegate OnLobbySettingsMenuShown;

        /// <summary>
        /// Gets invoked when lobby list menu has been shown
        /// </summary>
        event LobbyListMenuShownDelegate OnLobbyListMenuShown;

        /// <summary>
        /// Shows main menu
        /// </summary>
        void ShowMainMenu();

        /// <summary>
        /// Shows play menu
        /// </summary>
        void ShowPlayMenu();

        /// <summary>
        /// Shows gallery
        /// </summary>
        void ShowGallery();

        /// <summary>
        /// Shows credits
        /// </summary>
        void ShowCredits();

        /// <summary>
        /// Shows lobby settings menu
        /// </summary>
        void ShowLobbySettingsMenu();

        /// <summary>
        /// Shows lobby list menu
        /// </summary>
        void ShowLobbyListMenu();

        /// <summary>
        /// Plays game
        /// </summary>
        void Play();

        /// <summary>
        /// Creates a new lobby
        /// </summary>
        void CreateNewLobby();

        /// <summary>
        /// Joins selected lobby
        /// </summary>
        void JoinSelectedLobby();

        /// <summary>
        /// Quits game
        /// </summary>
        void QuitGame();
    }
}
