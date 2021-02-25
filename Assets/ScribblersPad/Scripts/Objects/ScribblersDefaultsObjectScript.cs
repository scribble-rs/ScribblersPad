using ScribblersSharp;
using System;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad objects namespace
/// </summary>
namespace ScribblersPad.Objects
{
    /// <summary>
    /// A class that describes a Scribble.rs default object script
    /// </summary>
    [CreateAssetMenu(fileName = "ScribblersDefaults", menuName = "Scribble.rs/Scribble.rs defaults")]
    public class ScribblersDefaultsObjectScript : ScriptableObject, IScribblersDefaultsObject
    {
        /// <summary>
        /// Default host
        /// </summary>
        public static readonly string defaultHost = "scribblers-official.herokuapp.com";

        /// <summary>
        /// Default lobby language
        /// </summary>
        public static readonly ELanguage defaultLobbyLanguage = ELanguage.EnglishUS;

        /// <summary>
        /// Host
        /// </summary>
        [SerializeField]
        private string host = defaultHost;

        /// <summary>
        /// Lobby language
        /// </summary>
        [SerializeField]
        private ELanguage lobbyLanguage = defaultLobbyLanguage;

        /// <summary>
        /// Host
        /// </summary>
        public string Host
        {
            get => host ?? defaultHost;
            set => host = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Lobby language
        /// </summary>
        public ELanguage LobbyLanguage
        {
            get => (lobbyLanguage == ELanguage.Invalid) ? defaultLobbyLanguage : lobbyLanguage;
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
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            host ??= defaultHost;
            lobbyLanguage = (lobbyLanguage == ELanguage.Invalid) ? defaultLobbyLanguage : lobbyLanguage;
        }
    }
}
