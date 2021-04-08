using ScribblersPad.Data;
using ScribblersSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnitySaveGame;
using UnitySceneLoaderManager;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a main menu controller script
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MainMenuControllerScript : MonoBehaviour, IMainMenuController
    {
        /// <summary>
        /// "nenuID" hash
        /// </summary>
        private static readonly int menuIDHash = Animator.StringToHash("menuID");

        /// <summary>
        /// Quit game title string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript quitGameTitleStringTranslation = default;

        /// <summary>
        /// Quit game message string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript quitGameMessageStringTranslation = default;

        /// <summary>
        /// Gets invoked when main menu has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onMainMenuShown = default;

        /// <summary>
        /// Gets invoked when play menu has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onPlayMenuShown = default;

        /// <summary>
        /// Gets invoked when lobby settings menu has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbySettingsMenuShown = default;

        /// <summary>
        /// Gets invoked when lobby list menu has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onLobbyListMenuShown = default;

        /// <summary>
        /// Main menu UI layout state
        /// </summary>
        private EMainMenuUILayoutState mainMenuUILayoutState = EMainMenuUILayoutState.Nothing;

        /// <summary>
        /// Play in lobby task
        /// </summary>
        private Task<ILobbyView> playInLobbyTask;

        /// <summary>
        /// Is pressing escape key
        /// </summary>
        private bool isPressingEscapeKey;

        /// <summary>
        /// Quit game title string translation
        /// </summary>
        public StringTranslationObjectScript QuitGameTitleStringTranslation
        {
            get => quitGameTitleStringTranslation;
            set => quitGameTitleStringTranslation = value;
        }

        /// <summary>
        /// Quit game message string translation
        /// </summary>
        public StringTranslationObjectScript QuitGameMessageStringTranslation
        {
            get => quitGameMessageStringTranslation;
            set => quitGameMessageStringTranslation = value;
        }

        /// <summary>
        /// Main menu UI layout state
        /// </summary>
        public EMainMenuUILayoutState MainMenuUILayoutState
        {
            get => mainMenuUILayoutState;
            set
            {
                if (mainMenuUILayoutState != value)
                {
                    switch (value)
                    {
                        case EMainMenuUILayoutState.Main:
                            mainMenuUILayoutState = EMainMenuUILayoutState.Main;
                            if (onMainMenuShown != null)
                            {
                                onMainMenuShown.Invoke();
                            }
                            OnMainMenuShown?.Invoke();
                            break;
                        case EMainMenuUILayoutState.Play:
                            mainMenuUILayoutState = EMainMenuUILayoutState.Play;
                            if (onPlayMenuShown != null)
                            {
                                onPlayMenuShown.Invoke();
                            }
                            OnPlayMenuShown?.Invoke();
                            break;
                        case EMainMenuUILayoutState.LobbySettings:
                            mainMenuUILayoutState = EMainMenuUILayoutState.LobbySettings;
                            if (onLobbySettingsMenuShown != null)
                            {
                                onLobbySettingsMenuShown.Invoke();
                            }
                            OnLobbySettingsMenuShown?.Invoke();
                            break;
                        case EMainMenuUILayoutState.LobbyList:
                            mainMenuUILayoutState = EMainMenuUILayoutState.LobbyList;
                            if (onLobbyListMenuShown != null)
                            {
                                onLobbyListMenuShown.Invoke();
                            }
                            OnLobbyListMenuShown?.Invoke();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Animator
        /// </summary>
        public Animator Animator { get; private set; }

        /// <summary>
        /// Gets invoked when main menu has been shown
        /// </summary>
        public event MainMenuShownDelegate OnMainMenuShown;

        /// <summary>
        /// Gets invoked when play menu has been shown
        /// </summary>
        public event PlayMenuShownDelegate OnPlayMenuShown;

        /// <summary>
        /// Gets invoked when lobby settings menu has been shown
        /// </summary>
        public event LobbySettingsMenuShownDelegate OnLobbySettingsMenuShown;

        /// <summary>
        /// Gets invoked when lobby list menu has been shown
        /// </summary>
        public event LobbyListMenuShownDelegate OnLobbyListMenuShown;

        /// <summary>
        /// Shows main menu
        /// </summary>
        public void ShowMainMenu() => MainMenuUILayoutState = EMainMenuUILayoutState.Main;

        /// <summary>
        /// Shows play menu
        /// </summary>
        public void ShowPlayMenu() => MainMenuUILayoutState = EMainMenuUILayoutState.Play;

        /// <summary>
        /// Shows gallery
        /// </summary>
        public void ShowGallery() => SceneLoaderManager.LoadScene("GalleryMenuScene");

        /// <summary>
        /// Shows credits
        /// </summary>
        public void ShowCredits() => SceneLoaderManager.LoadScene("CreditsMenuScene");

        /// <summary>
        /// Shows lobby settings menu
        /// </summary>
        public void ShowLobbySettingsMenu() => MainMenuUILayoutState = EMainMenuUILayoutState.LobbySettings;

        /// <summary>
        /// Shows lobby list menu
        /// </summary>
        public void ShowLobbyListMenu() => MainMenuUILayoutState = EMainMenuUILayoutState.LobbyList;

        /// <summary>
        /// Plays game
        /// </summary>
        public void Play()
        {
            if (playInLobbyTask != null)
            {
                // TODO: Invoke events
                playInLobbyTask = null;
            }
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                string host = save_game.Data.ScribblersHost;
                string user_session_id = save_game.Data.GetUserSessionID(host);
                bool is_using_secure_protocols = save_game.Data.IsUsingSecureProtocols;
                bool is_allowed_to_use_insecure_protocols = save_game.Data.IsAllowedToUseInsecureProtocols;
                ELanguage language = save_game.Data.LobbyLanguage;
                uint round_count = save_game.Data.RoundCount;
                uint drawing_time = save_game.Data.DrawingTime;
                bool is_votekicking_enabled = save_game.Data.IsVotekickingEnabled;
                uint maximal_player_count = save_game.Data.MaximalPlayerCount;
                uint players_per_ip_limit = save_game.Data.PlayersPerIPLimit;
                playInLobbyTask = Task.Run(async () =>
                {
                    ILobbyView result = null;
                    try
                    {
                        using (IScribblersClient scribblers_client = Clients.Create(host, user_session_id, is_using_secure_protocols, is_allowed_to_use_insecure_protocols))
                        {
                            IEnumerable<ILobbyView> lobby_views = await scribblers_client.ListLobbiesAsync();
                            if (lobby_views != null)
                            {
                                List<ILobbyView> candidate_lobby_views = new List<ILobbyView>();
                                foreach (ILobbyView lobby_view in lobby_views)
                                {
                                    if
                                    (
                                        (lobby_view.PlayerCount < lobby_view.MaximalPlayerCount) &&
                                        (lobby_view.Language == language) &&
                                        (lobby_view.RoundCount == round_count) &&
                                        (lobby_view.DrawingTime == drawing_time) &&
                                        (lobby_view.IsVotekickingEnabled == is_votekicking_enabled) &&
                                        (lobby_view.MaximalPlayerCount == maximal_player_count) &&
                                        (lobby_view.MaximalClientsPerIPCount == players_per_ip_limit)
                                    )
                                    {
                                        candidate_lobby_views.Add(lobby_view);
                                    }
                                }
                                if (candidate_lobby_views.Count <= 0)
                                {
                                    foreach (ILobbyView lobby_view in lobby_views)
                                    {
                                        if
                                        (
                                            (lobby_view.PlayerCount < lobby_view.MaximalPlayerCount) &&
                                            (lobby_view.Language == language) &&
                                            (lobby_view.RoundCount == round_count)
                                        )
                                        {
                                            candidate_lobby_views.Add(lobby_view);
                                        }
                                    }
                                }
                                if (candidate_lobby_views.Count <= 0)
                                {
                                    foreach (ILobbyView lobby_view in lobby_views)
                                    {
                                        if
                                        (
                                            (lobby_view.PlayerCount < lobby_view.MaximalPlayerCount) &&
                                            (lobby_view.Language == language)
                                        )
                                        {
                                            candidate_lobby_views.Add(lobby_view);
                                        }
                                    }
                                }
                                if (candidate_lobby_views.Count > 0)
                                {
                                    result = candidate_lobby_views[new System.Random().Next(0, candidate_lobby_views.Count)];
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    return result;
                });
            }
        }

        /// <summary>
        /// Creates a new lobby
        /// </summary>
        public void CreateNewLobby()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                save_game.Data.LobbyID = string.Empty;
                save_game.Save();
                SceneLoaderManager.LoadScene("GameScene");
            }
        }

        /// <summary>
        /// Joins selected lobby
        /// </summary>
        public void JoinSelectedLobby()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if ((save_game != null) && !string.IsNullOrWhiteSpace(save_game.Data.LobbyID))
            {
                SceneLoaderManager.LoadScene("GameScene");
            }
        }

        /// <summary>
        /// Requests quitting game
        /// </summary>
        public void RequestQuittingGame()
        {
            Dialogs.Show(quitGameTitleStringTranslation ? quitGameTitleStringTranslation.ToString() : string.Empty, quitGameMessageStringTranslation ? quitGameMessageStringTranslation.ToString() : string.Empty, EDialogType.Information, EDialogButtons.YesNo, (dialogResponse, _) =>
            {
                if (dialogResponse == EDialogResponse.Yes)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            });
        }

        /// <summary>
        /// Gets invoked when script gets started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out Animator animator))
            {
                Animator = animator;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(UnityEngine.Animator) }\" component to this game object.", this);
            }
            MainMenuUILayoutState = EMainMenuUILayoutState.Main;
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if ((keyboard != null) && isPressingEscapeKey && !keyboard.escapeKey.isPressed)
            {
                switch (mainMenuUILayoutState)
                {
                    case EMainMenuUILayoutState.Nothing:
                    case EMainMenuUILayoutState.Main:
                        RequestQuittingGame();
                        break;
                    case EMainMenuUILayoutState.Play:
                        ShowMainMenu();
                        break;
                    case EMainMenuUILayoutState.LobbySettings:
                    case EMainMenuUILayoutState.LobbyList:
                        ShowPlayMenu();
                        break;
                }
            }
            isPressingEscapeKey = keyboard.escapeKey.isPressed;
            if (playInLobbyTask != null)
            {
                switch (playInLobbyTask.Status)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        playInLobbyTask = null;
                        break;
                    case TaskStatus.RanToCompletion:
                        SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                        if (save_game != null)
                        {
                            ILobbyView lobby_view = playInLobbyTask.Result;
                            if (lobby_view == null)
                            {
                                CreateNewLobby();
                            }
                            else
                            {
                                save_game.Data.LobbyID = lobby_view.LobbyID;
                                save_game.Save();
                                JoinSelectedLobby();
                            }
                        }
                        playInLobbyTask = null;
                        break;
                }
            }
            if (Animator)
            {
                Animator.SetInteger(menuIDHash, (int)mainMenuUILayoutState);
            }
        }
    }
}
