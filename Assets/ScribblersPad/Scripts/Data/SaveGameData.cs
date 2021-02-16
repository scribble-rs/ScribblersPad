using ScribblersPad.Objects;
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
        /// Scribble.rs lobby ID
        /// </summary>
        [SerializeField]
        private string scribblersLobbyID;

        /// <summary>
        /// Scribble.rs username
        /// </summary>
        [SerializeField]
        private string scribblersUsername;

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
        /// Scribble.rs lobby ID
        /// </summary>
        public string ScribblersLobbyID
        {
            get => scribblersLobbyID ?? string.Empty;
            set => scribblersLobbyID = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Scribble.rs username
        /// </summary>
        public string ScribblersUsername
        {
            get => scribblersUsername ?? string.Empty;
            set => scribblersUsername = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Cnstructs save game data
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
                scribblersLobbyID = save_game_data.scribblersLobbyID;
                scribblersUsername = save_game_data.scribblersUsername;
            }
        }
    }
}
