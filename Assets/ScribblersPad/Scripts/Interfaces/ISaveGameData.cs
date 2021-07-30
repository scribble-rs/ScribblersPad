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
        /// Is using secure protocols
        /// </summary>
        bool IsUsingSecureProtocols { get; set; }

        /// <summary>
        /// Is allowed to use insecure protocols
        /// </summary>
        bool IsAllowedToUseInsecureProtocols { get; set; }

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

        /// <summary>
        /// Gets user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>User session ID</returns>
        string GetUserSessionID(string host);

        /// <summary>
        /// TRies to get user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="userSessionID">User session ID</param>
        /// <returns>"true" if user session ID is available, otherwise "false"</returns>
        bool TryGetUserSessionID(string host, out string userSessionID);

        /// <summary>
        /// Sets an user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="userSessionID">User session ID</param>
        void SetUserSessionID(string host, string userSessionID);

        /// <summary>
        /// Removes an user session ID
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool RemoveUserSessionID(string host);

        /// <summary>
        /// Clears user session IDs
        /// </summary>
        void ClearUserSessionIDs();
    }
}
