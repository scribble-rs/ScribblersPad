using ScribblersPad.Data;
using ScribblersSharp;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        /// Language sprites
        /// </summary>
        [SerializeField]
        private LanguageSpriteData[] languageSprites = Array.Empty<LanguageSpriteData>();

        /// <summary>
        /// Unknown language sprite
        /// </summary>
        [SerializeField]
        private Sprite unknownLanguageSprite = default;

        /// <summary>
        /// Is votekicking disabled sprite
        /// </summary>
        [SerializeField]
        private Sprite isVotekickingDisabledSprite = default;

        /// <summary>
        /// Is votekicking enabled sprite
        /// </summary>
        [SerializeField]
        private Sprite isVotekickingEnabledSprite = default;

        /// <summary>
        /// Is not using custom words sprite
        /// </summary>
        [SerializeField]
        private Sprite isNotUsingCustomWordsSprite = default;

        /// <summary>
        /// Is using custom words sprite
        /// </summary>
        [SerializeField]
        private Sprite isUsingCustomWordsSprite = default;

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
        /// Language image
        /// </summary>
        [SerializeField]
        private Image languageImage = default;

        /// <summary>
        /// Is votekicking enabled image
        /// </summary>
        [SerializeField]
        private Image isVotekickingEnabledImage = default;

        /// <summary>
        /// Is using custom words image
        /// </summary>
        [SerializeField]
        private Image isUsingCustomWordsImage = default;

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
        /// Unknown language sprite
        /// </summary>
        public Sprite UnknownLanguageSprite
        {
            get => unknownLanguageSprite;
            set => unknownLanguageSprite = value;
        }

        /// <summary>
        /// Is votekicking disabled sprite
        /// </summary>
        public Sprite IsVotekickingDisabledSprite
        {
            get => isVotekickingDisabledSprite;
            set => isVotekickingDisabledSprite = value;
        }

        /// <summary>
        /// Is votekicking enabled sprite
        /// </summary>
        public Sprite IsVotekickingEnabledSprite
        {
            get => isVotekickingEnabledSprite;
            set => isVotekickingEnabledSprite = value;
        }

        /// <summary>
        /// Is not using custom words sprite
        /// </summary>
        public Sprite IsNotUsingCustomWordsSprite
        {
            get => isNotUsingCustomWordsSprite;
            set => isNotUsingCustomWordsSprite = value;
        }

        /// <summary>
        /// Is using custom words sprite
        /// </summary>
        public Sprite IsUsingCustomWordsSprite
        {
            get => isUsingCustomWordsSprite;
            set => isUsingCustomWordsSprite = value;
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
        /// Language image
        /// </summary>
        public Image LanguageImage
        {
            get => languageImage;
            set => languageImage = value;
        }

        /// <summary>
        /// Is votekicking enabled image
        /// </summary>
        public Image IsVotekickingEnabledImage
        {
            get => isVotekickingEnabledImage;
            set => isVotekickingEnabledImage = value;
        }

        /// <summary>
        /// Is using custom words image
        /// </summary>
        public Image OsUsingCustomWordsImage
        {
            get => isUsingCustomWordsImage;
            set => isUsingCustomWordsImage = value;
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
            if (languageImage)
            {
                languageImage.sprite = languageSpriteLookup.ContainsKey(lobbyView.Language) ? languageSpriteLookup[lobbyView.Language] : unknownLanguageSprite;
            }
            if (isVotekickingEnabledImage)
            {
                isVotekickingEnabledImage.sprite = lobbyView.IsVotekickingEnabled ? isVotekickingEnabledSprite : isVotekickingDisabledSprite;
            }
            if (isUsingCustomWordsImage)
            {
                isUsingCustomWordsImage.sprite = lobbyView.IsUsingCustomWords ? isUsingCustomWordsSprite : isNotUsingCustomWordsSprite;
            }
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
    }
}
