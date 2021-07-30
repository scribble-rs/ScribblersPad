using ScribblersPad.Objects;
using ScribblersSharp;
using System;
using System.Collections.Generic;
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
        /// User session IDs
        /// </summary>
        [SerializeField]
        private string[] userSessionIDs;

        /// <summary>
        /// Is using secure protocols
        /// </summary>
        [SerializeField]
        private bool isUsingSecureProtocols = true;

        /// <summary>
        /// Is allowed to use insecure protocols
        /// </summary>
        [SerializeField]
        private bool isAllowedToUseInsecureProtocols = false;

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

        private Dictionary<string, string> userSessionIDLookup;

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
        /// Is using secure protocols
        /// </summary>
        public bool IsUsingSecureProtocols
        {
            get => isUsingSecureProtocols;
            set => isUsingSecureProtocols = value;
        }

        /// <summary>
        /// Is allowed to use insecure protocols
        /// </summary>
        public bool IsAllowedToUseInsecureProtocols
        {
            get => isAllowedToUseInsecureProtocols;
            set => isAllowedToUseInsecureProtocols = value;
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
                userSessionIDs = (save_game_data.userSessionIDs == null) ? null : save_game_data.userSessionIDs.Clone() as string[];
                isUsingSecureProtocols = save_game_data.isUsingSecureProtocols;
                isAllowedToUseInsecureProtocols = save_game_data.isAllowedToUseInsecureProtocols;
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

        private void InitializeUserSessionIDLookup()
        {
            if (userSessionIDLookup == null)
            {
                userSessionIDLookup = new Dictionary<string, string>();
                if (userSessionIDs != null)
                {
                    foreach (string user_session_id in userSessionIDs)
                    {
                        if (string.IsNullOrWhiteSpace(user_session_id))
                        {
                            Debug.LogError($"Session ID entry is null.");
                        }
                        else
                        {
                            string[] user_session_id_strings = user_session_id.Split('=');
                            if (user_session_id_strings.Length > 1)
                            {
                                string host = user_session_id_strings[0];
                                if (userSessionIDLookup.ContainsKey(host))
                                {
                                    Debug.LogError($"Found duplicate user session ID for \"{ host }\" in save game.");
                                }
                                else
                                {
                                    userSessionIDLookup.Add(host, string.Join("=", user_session_id_strings, 1, user_session_id_strings.Length - 1));
                                }
                            }
                            else
                            {
                                Debug.LogError($"\"{ user_session_id }\" is not a valid session ID entry.");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates user session IDs
        /// </summary>
        private void UpdateUserSessionIDs()
        {
            if (userSessionIDs.Length != userSessionIDLookup.Count)
            {
                userSessionIDs = new string[userSessionIDLookup.Count];
            }
            int index = 0;
            foreach (KeyValuePair<string, string> user_session_id_lookup_pair in userSessionIDLookup)
            {
                userSessionIDs[index] = $"{ user_session_id_lookup_pair.Key }={ user_session_id_lookup_pair.Value }";
                ++index;
            }
        }

        /// <summary>
        /// Gets user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>User session ID</returns>
        public string GetUserSessionID(string host) => TryGetUserSessionID(host, out string ret) ? ret : string.Empty;

        /// <summary>
        /// TRies to get user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="userSessionID">User session ID</param>
        /// <returns>"true" if user session ID is available, otherwise "false"</returns>
        public bool TryGetUserSessionID(string host, out string userSessionID)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            InitializeUserSessionIDLookup();
            bool ret = userSessionIDLookup.TryGetValue(host, out string result);
            userSessionID = ret ? result : string.Empty;
            return ret;
        }

        /// <summary>
        /// Sets an user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="userSessionID">User session ID</param>
        public void SetUserSessionID(string host, string userSessionID)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (userSessionID == null)
            {
                throw new ArgumentNullException(nameof(userSessionID));
            }
            InitializeUserSessionIDLookup();
            if (userSessionIDLookup.ContainsKey(host))
            {
                userSessionIDLookup[host] = userSessionID;
            }
            else
            {
                userSessionIDLookup.Add(host, userSessionID);
            }
            UpdateUserSessionIDs();
        }

        /// <summary>
        /// Removes an user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveUserSessionID(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            InitializeUserSessionIDLookup();
            bool ret = userSessionIDLookup.Remove(host);
            if (ret)
            {
                UpdateUserSessionIDs();
            }
            return ret;
        }

        /// <summary>
        /// Clears user session IDs
        /// </summary>
        public void ClearUserSessionIDs()
        {
            InitializeUserSessionIDLookup();
            userSessionIDLookup?.Clear();
            userSessionIDs = Array.Empty<string>();
        }
    }
}
