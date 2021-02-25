using ScribblersPad.Objects;
using ScribblersSharp;
using System;
using UnityEngine;
using UnitySaveGame;

/// <summary>
/// Scribble.rs Pad data namespace
/// </summary>
namespace ScribblersPad.Data
{
    /// <summary>
    /// A class that describes save game data
    /// </summary>
    [Serializable]
    public class SaveGameData : ASaveGameData, ISaveGameData
    {
        /// <summary>
        /// Scribble.rs host
        /// </summary>
        [SerializeField]
        private string scribblersHost;

        /// <summary>
        /// User session ID
        /// </summary>
        [SerializeField]
        private string userSessionID;

        /// <summary>
        /// Is using secure protocols
        /// </summary>
        [SerializeField]
        private bool isUsingSecureProtocols = true;

        /// <summary>
        /// Lobby ID
        /// </summary>
        [SerializeField]
        private string lobbyID;

        /// <summary>
        /// Username
        /// </summary>
        [SerializeField]
        private string username;

        /// <summary>
        /// Lobby language
        /// </summary>
        [SerializeField]
        private ELanguage lobbyLanguage;

        /// <summary>
        /// Drawing time in seconds
        /// </summary>
        [SerializeField]
        private uint drawingTime = 120U;

        /// <summary>
        /// Round count
        /// </summary>
        [SerializeField]
        private uint roundCount = 4U;

        /// <summary>
        /// Maximal player count
        /// </summary>
        [SerializeField]
        private uint maximalPlayerCount = 12U;

        /// <summary>
        /// Is lobby public
        /// </summary>
        [SerializeField]
        private bool isLobbyPublic = true;

        /// <summary>
        /// Custom words
        /// </summary>
        [SerializeField]
        private string customWords = string.Empty;

        /// <summary>
        /// Custom words chance
        /// </summary>
        [SerializeField]
        private uint customWordsChance = 50U;

        /// <summary>
        /// Players per IP limit
        /// </summary>
        [SerializeField]
        private uint playersPerIPLimit = 8U;

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        [SerializeField]
        private bool isVotekickingEnabled = true;

        /// <summary>
        /// Save game defaults
        /// </summary>
        private ScribblersDefaultsObjectScript defaults;

        /// <summary>
        /// Save game defaults
        /// </summary>
        private ScribblersDefaultsObjectScript Defaults => defaults ??= Resources.Load<ScribblersDefaultsObjectScript>("Defaults/ScribblersDefaults");

        /// <summary>
        /// Scribble.rs host
        /// </summary>
        public string ScribblersHost
        {
            get
            {
                if (string.IsNullOrWhiteSpace(scribblersHost))
                {
                    scribblersHost = Defaults ? Defaults.Host : ScribblersDefaultsObjectScript.defaultHost;
                }
                return scribblersHost;
            }
            set => scribblersHost = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// User session ID
        /// </summary>
        public string UserSessionID
        {
            get => userSessionID ?? string.Empty;
            set => userSessionID = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Is using secure protocols
        /// </summary>
        public bool IsUsingSecureProtocols
        {
            get => isUsingSecureProtocols;
            set => isUsingSecureProtocols = value;
        }

        /// <summary>
        /// Lobby ID
        /// </summary>
        public string LobbyID
        {
            get => lobbyID ?? string.Empty;
            set => lobbyID = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Username
        /// </summary>
        public string Username
        {
            get => username ?? string.Empty;
            set => username = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Lobby language
        /// </summary>
        public ELanguage LobbyLanguage
        {
            get => (lobbyLanguage == ELanguage.Invalid) ? Defaults.LobbyLanguage : lobbyLanguage;
            set
            {
                if (value == ELanguage.Invalid)
                {
                    throw new ArgumentException("Lobby language can't be invalid.", nameof(value));
                }
                lobbyLanguage = value;
            }
        }

        /// <summary>
        /// Drawing time in seconds
        /// </summary>
        public uint DrawingTime
        {
            get => drawingTime;
            set => drawingTime = value;
        }

        /// <summary>
        /// Round count
        /// </summary>
        public uint RoundCount
        {
            get => roundCount;
            set => roundCount = value;
        }

        /// <summary>
        /// Maximal player count
        /// </summary>
        public uint MaximalPlayerCount
        {
            get => maximalPlayerCount;
            set => maximalPlayerCount = value;
        }

        /// <summary>
        /// Is lobby public
        /// </summary>
        public bool IsLobbyPublic
        {
            get => isLobbyPublic;
            set => isLobbyPublic = value;
        }

        /// <summary>
        /// Custom words
        /// </summary>
        public string CustomWords
        {
            get => customWords ?? string.Empty;
            set => customWords = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Custom words chance
        /// </summary>
        public uint CustomWordsChance
        {
            get => customWordsChance;
            set => customWordsChance = value;
        }

        /// <summary>
        /// Players per IP limit
        /// </summary>
        public uint PlayersPerIPLimit
        {
            get => playersPerIPLimit;
            set => playersPerIPLimit = value;
        }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        public bool IsVotekickingEnabled
        {
            get => isVotekickingEnabled;
            set => isVotekickingEnabled = value;
        }

        /// <summary>
        /// Constructs save game data
        /// </summary>
        public SaveGameData() : base(null)
        {
            // ...
        }

        /// <summary>
        /// Constructs save game data
        /// </summary>
        /// <param name="saveGameData">Save game data</param>
        public SaveGameData(ASaveGameData saveGameData) : base(saveGameData)
        {
            if (saveGameData is SaveGameData save_game_data)
            {
                scribblersHost = save_game_data.scribblersHost;
                userSessionID = save_game_data.userSessionID;
                isUsingSecureProtocols = save_game_data.isUsingSecureProtocols;
                lobbyID = save_game_data.lobbyID;
                username = save_game_data.username;
                lobbyLanguage = save_game_data.lobbyLanguage;
                drawingTime = save_game_data.drawingTime;
                roundCount = save_game_data.roundCount;
                maximalPlayerCount = save_game_data.maximalPlayerCount;
                isLobbyPublic = save_game_data.isLobbyPublic;
                customWords = save_game_data.customWords;
                customWordsChance = save_game_data.customWordsChance;
                playersPerIPLimit = save_game_data.playersPerIPLimit;
                isVotekickingEnabled = save_game_data.isVotekickingEnabled;
            }
        }
    }
}
