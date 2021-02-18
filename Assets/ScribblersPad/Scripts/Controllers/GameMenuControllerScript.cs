using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a game menu controller script
    /// </summary>
    public class GameMenuControllerScript : MonoBehaviour, IGameMenuController
    {
        /// <summary>
        /// Exit game question title string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript exitGameQuestionTitleStringTranslation = default;

        /// <summary>
        /// Exit game question message string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript exitGameQuestionMessageStringTranslation = default;

        /// <summary>
        /// Gets invoked when game menu has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onGameMenuShown = default;

        /// <summary>
        /// Gets invoked when game menu has been hidden
        /// </summary>
        [SerializeField]
        private UnityEvent onGameMenuHidden = default;

        /// <summary>
        /// Is showing game menu
        /// </summary>
        private bool isShowingGameMenu;

        /// <summary>
        /// Is pressing escape key
        /// </summary>
        private bool isPressingEscapeKey;

        /// <summary>
        /// Exit game question title string translation
        /// </summary>
        public StringTranslationObjectScript ExitGameQuestionTitleStringTranslation
        {
            get => exitGameQuestionTitleStringTranslation;
            set => exitGameQuestionTitleStringTranslation = value;
        }

        /// <summary>
        /// Exit game question message string translation
        /// </summary>
        public StringTranslationObjectScript ExitGameQuestionMessageStringTranslation
        {
            get => exitGameQuestionMessageStringTranslation;
            set => exitGameQuestionMessageStringTranslation = value;
        }

        /// <summary>
        /// Is showing game menu
        /// </summary>
        public bool IsShowingGameMenu
        {
            get => isShowingGameMenu;
            set
            {
                if (isShowingGameMenu != value)
                {
                    isShowingGameMenu = value;
                    if (isShowingGameMenu)
                    {
                        if (onGameMenuShown != null)
                        {
                            onGameMenuShown.Invoke();
                        }
                        OnGameMenuShown?.Invoke();
                    }
                    else
                    {
                        if (onGameMenuHidden != null)
                        {
                            onGameMenuHidden.Invoke();
                        }
                        OnGameMenuHidden?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Gets invoked when game menu has been shown
        /// </summary>
        public event GameMenuShownDelegate OnGameMenuShown;

        /// <summary>
        /// Gets invoked when game menu has been hidden
        /// </summary>
        public event GameMenuHiddenDelegate OnGameMenuHidden;

        /// <summary>
        /// Requests exiting game
        /// </summary>
        public void RequestExitingGame()
        {
            Dialog.Show((exitGameQuestionTitleStringTranslation == null) ? string.Empty : exitGameQuestionTitleStringTranslation.ToString(), (exitGameQuestionMessageStringTranslation == null) ? string.Empty : exitGameQuestionMessageStringTranslation.ToString(), EDialogType.Information, EDialogButtons.YesNo, (e) =>
            {
                if (e.DialogResponse == EDialogResponse.Yes)
                {
                    IsShowingGameMenu = false;
                    SceneLoaderManager.LoadScene("MainMenuScene");
                }
            });
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if ((keyboard != null) && isPressingEscapeKey && !keyboard.escapeKey.isPressed)
            {
                IsShowingGameMenu = !isShowingGameMenu;
            }
            isPressingEscapeKey = keyboard.escapeKey.isPressed;
        }
    }
}
