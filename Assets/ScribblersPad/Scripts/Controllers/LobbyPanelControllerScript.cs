using ScribblersPad.Data;
using ScribblersSharp;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnitySaveGame;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a lobby panel controller script
    /// </summary>
    public class LobbyPanelControllerScript : MonoBehaviour, ILobbyPanelController
    {
        /// <summary>
        /// Default current round string format
        /// </summary>
        private static readonly string defaultCurrentRoundStringFormat = "{0}/{1}";

        /// <summary>
        /// Default player count string format
        /// </summary>
        private static readonly string defaultPlayerCountStringFormat = "{0}/{1}";

        /// <summary>
        /// "languageIndex" hash
        /// </summary>
        private static readonly int languageIndexHash = Animator.StringToHash("languageIndex");

        /// <summary>
        /// "isVotekickingEnabled" hash
        /// </summary>
        private static readonly int isVotekickingEnabledHash = Animator.StringToHash("isVotekickingEnabled");

        /// <summary>
        /// "isUsingCustomWords" hash
        /// </summary>
        private static readonly int isUsingCustomWordsHash = Animator.StringToHash("isUsingCustomWords");

        /// <summary>
        /// Language sprites
        /// </summary>
        [SerializeField]
        private LanguageSpriteData[] languageSprites = Array.Empty<LanguageSpriteData>();

        /// <summary>
        /// Language image animator
        /// </summary>
        [SerializeField]
        private Animator languageImageAnimator = default;

        /// <summary>
        /// Is votekicking enabled image animator
        /// </summary>
        [SerializeField]
        private Animator isVotekickingEnabledImageAnimator = default;

        /// <summary>
        /// Is using custom words image animator
        /// </summary>
        [SerializeField]
        private Animator isUsingCustomWordsImageAnimator = default;

        /// <summary>
        /// Current round string format
        /// </summary>
        [SerializeField]
        private string currentRoundStringFormat = defaultCurrentRoundStringFormat;

        /// <summary>
        /// Current round string format string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript currentRoundStringFormatStringTranslation = default;

        /// <summary>
        /// Player count string format
        /// </summary>
        [SerializeField]
        private string playerCountStringFormat = defaultPlayerCountStringFormat;

        /// <summary>
        /// Player count string format string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript playerCountStringFormatStringTranslation = default;

        /// <summary>
        /// Current round text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI currentRoundText = default;

        /// <summary>
        /// Player count text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI playerCountText = default;

        /// <summary>
        /// Language sprite lookup
        /// </summary>
        private Dictionary<ELanguage, Sprite> languageSpriteLookup;

        /// <summary>
        /// Language image animator
        /// </summary>
        public Animator LanguageImageAnimator
        {
            get => languageImageAnimator;
            set => languageImageAnimator = value;
        }

        /// <summary>
        /// Is votekicking enabled image animator
        /// </summary>
        public Animator IsVotekickingEnabledImageAnimator
        {
            get => isVotekickingEnabledImageAnimator;
            set => isVotekickingEnabledImageAnimator = value;
        }

        /// <summary>
        /// Is using custom words image animator
        /// </summary>
        public Animator IsUsingCustomWordsImageAnimator
        {
            get => isUsingCustomWordsImageAnimator;
            set => isUsingCustomWordsImageAnimator = value;
        }

        /// <summary>
        /// Current round string format
        /// </summary>
        public string CurrentRoundStringFormat
        {
            get => currentRoundStringFormat ?? defaultCurrentRoundStringFormat;
            set => currentRoundStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Current round string format string translation
        /// </summary>
        public StringTranslationObjectScript CurrentRoundStringFormatStringTranslation
        {
            get => currentRoundStringFormatStringTranslation;
            set => currentRoundStringFormatStringTranslation = value;
        }

        /// <summary>
        /// Player count string format
        /// </summary>
        public string PlayerCountStringFormat
        {
            get => playerCountStringFormat ?? defaultPlayerCountStringFormat;
            set => playerCountStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Player count string format string translation
        /// </summary>
        public StringTranslationObjectScript PlayerCountStringFormatStringTranslation
        {
            get => playerCountStringFormatStringTranslation;
            set => playerCountStringFormatStringTranslation = value;
        }

        /// <summary>
        /// Current round text
        /// </summary>
        public TextMeshProUGUI CurrentRoundText
        {
            get => currentRoundText;
            set => currentRoundText = value;
        }

        /// <summary>
        /// Player count text
        /// </summary>
        public TextMeshProUGUI PlayerCountText
        {
            get => playerCountText;
            set => playerCountText = value;
        }

        /// <summary>
        /// Lobby view
        /// </summary>
        public ILobbyView LobbyView { get; private set; }

        /// <summary>
        /// Initializes language translations
        /// </summary>
        private void InitializeLanguageTranslations()
        {
            if (languageSpriteLookup == null)
            {
                languageSpriteLookup = new Dictionary<ELanguage, Sprite>();
                if (languageSprites != null)
                {
                    foreach (LanguageSpriteData language_translation in languageSprites)
                    {
                        if (language_translation.Language == ELanguage.Invalid)
                        {
                            Debug.LogError("Found invalid language entry.", this);
                        }
                        else if (languageSpriteLookup.ContainsKey(language_translation.Language))
                        {
                            Debug.LogWarning($"Skipping duplicate language translation entry { language_translation.Language }...", this);
                        }
                        else
                        {
                            languageSpriteLookup.Add(language_translation.Language, language_translation.Sprite);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets values for component
        /// </summary>
        /// <param name="lobbyView">Lobby view</param>
        public void SetValues(ILobbyView lobbyView)
        {
            LobbyView = lobbyView ?? throw new ArgumentNullException(nameof(lobbyView));
            InitializeLanguageTranslations();
            if (currentRoundText)
            {
                currentRoundText.text = string.Format(currentRoundStringFormatStringTranslation ? currentRoundStringFormatStringTranslation.ToString() : currentRoundStringFormat, lobbyView.RoundCount, lobbyView.MaximalRoundCount);
            }
            if (playerCountText)
            {
                playerCountText.text = string.Format(playerCountStringFormatStringTranslation ? playerCountStringFormatStringTranslation.ToString() : playerCountStringFormat, lobbyView.PlayerCount, lobbyView.MaximalPlayerCount);
            }
        }

        /// <summary>
        /// Joins lobby
        /// </summary>
        public void JoinLobby()
        {
            if (LobbyView != null)
            {
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    save_game.Data.LobbyID = LobbyView.LobbyID;
                    save_game.Save();
                    UnitySceneLoaderManager.SceneLoaderManager.LoadScene("GameScene");
                }
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (currentRoundStringFormatStringTranslation)
            {
                currentRoundStringFormat = currentRoundStringFormatStringTranslation.ToString();
            }
            if (playerCountStringFormatStringTranslation)
            {
                playerCountStringFormat = playerCountStringFormatStringTranslation.ToString();
            }
        }

        private void Update()
        {
            if (languageImageAnimator)
            {
                languageImageAnimator.SetInteger(languageIndexHash, (int)((LobbyView == null) ? ELanguage.Invalid : LobbyView.Language));
            }
            if (isVotekickingEnabledImageAnimator)
            {
                isVotekickingEnabledImageAnimator.SetBool(isVotekickingEnabledHash, (LobbyView != null) && LobbyView.IsVotekickingEnabled);
            }
            if (isUsingCustomWordsImageAnimator)
            {
                isUsingCustomWordsImageAnimator.SetBool(isUsingCustomWordsHash, (LobbyView != null) && LobbyView.IsUsingCustomWords);
            }
        }
    }
}
