using ScribblersSharp;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// A class that represents save game data
    /// </summary>
    public interface ISaveGameData
    {
        /// <summary>
        /// Scribble.rs host
        /// </summary>
        string ScribblersHost { get; set; }

        /// <summary>
        /// User session ID
        /// </summary>
        string UserSessionID { get; set; }

        /// <summary>
        /// Is using secure protocols
        /// </summary>
        bool IsUsingSecureProtocols { get; set; }

        /// <summary>
        /// Lobby ID
        /// </summary>
        string LobbyID { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Lobby language
        /// </summary>
        ELanguage LobbyLanguage { get; set; }

        /// <summary>
        /// Drawing time in seconds
        /// </summary>
        uint DrawingTime { get; set; }

        /// <summary>
        /// Round count
        /// </summary>
        uint RoundCount { get; set; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        uint MaximalPlayerCount { get; set; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        bool IsLobbyPublic { get; set; }

        /// <summary>
        /// Custom words
        /// </summary>
        string CustomWords { get; set; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        uint CustomWordsChance { get; set; }

        /// <summary>
        /// Players per IP limit
        /// </summary>
        uint PlayersPerIPLimit { get; set; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        bool IsVotekickingEnabled { get; set; }
    }
}
