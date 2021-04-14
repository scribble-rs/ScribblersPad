using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a game menu controller
    /// </summary>
    public interface IGameMenuController : IBehaviour
    {
        /// <summary>
        /// Exit game question title string translation
        /// </summary>
        StringTranslationObjectScript ExitGameQuestionTitleStringTranslation { get; set; }

        /// <summary>
        /// Exit game question message string translation
        /// </summary>
        StringTranslationObjectScript ExitGameQuestionMessageStringTranslation { get; set; }

        /// <summary>
        /// Is showing game menu
        /// </summary>
        bool IsShowingGameMenu { get; set; }

        /// <summary>
        /// Gets invoked when game menu has been shown
        /// </summary>
        event GameMenuShownDelegate OnGameMenuShown;

        /// <summary>
        /// Gets invoked when game menu has been hidden
        /// </summary>
        event GameMenuHiddenDelegate OnGameMenuHidden;

        /// <summary>
        /// Shows main menu
        /// </summary>
        void RequestExitingGame();
    }
}
