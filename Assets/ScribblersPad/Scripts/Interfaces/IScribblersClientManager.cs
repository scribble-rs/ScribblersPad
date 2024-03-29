﻿using ScribblersSharp;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a Scribble.rs client manager
    /// </summary>
    public interface IScribblersClientManager : ISingletonManager
    {
        /// <summary>
        /// Scribble.rs client
        /// </summary>
        IScribblersClient ScribblersClient { get; }

        /// <summary>
        /// Lobby
        /// </summary>
        ILobby Lobby { get; }

        /// <summary>
        /// Web socket state
        /// </summary>
        WebSocketState WebSocketState { get; }

        /// <summary>
        /// Lobby ID
        /// </summary>
        string LobbyID { get; }

        /// <summary>
        /// Lobby limits
        /// </summary>
        ILobbyLimits Limits { get; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        uint MaximalPlayerCount { get; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        bool IsLobbyPublic { get; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        bool IsVotekickingEnabled { get; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        uint CustomWordsChance { get; }

        /// <summary>
        /// Allowed clients per IP count
        /// </summary>
        uint AllowedClientsPerIPCount { get; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        uint DrawingBoardBaseWidth { get; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        uint DrawingBoardBaseHeight { get; }

        /// <summary>
        /// Suggested brush sizes
        /// </summary>
        IEnumerable<uint> SuggestedBrushSizes { get; }

        /// <summary>
        /// Canvas color
        /// </summary>
        Color32 CanvasColor { get; }

        /// <summary>
        /// My player in lobby
        /// </summary>
        IPlayer MyPlayer { get; }

        /// <summary>
        /// Is my player allowed to draw
        /// </summary>
        bool IsPlayerAllowedToDraw { get; }

        /// <summary>
        /// Lobby owner
        /// </summary>
        IPlayer Owner { get; }

        /// <summary>
        /// Current round
        /// </summary>
        uint CurrentRound { get; }

        /// <summary>
        /// Current maximal round count
        /// </summary>
        uint CurrentMaximalRoundCount { get; }

        /// <summary>
        /// Current drawing time in milliseconds
        /// </summary>
        long CurrentDrawingTime { get; }

        /// <summary>
        /// Previous word
        /// </summary>
        string PreviousWord { get; }

        /// <summary>
        /// Word hints
        /// </summary>
        IReadOnlyList<IWordHint> WordHints { get; }

        /// <summary>
        /// Lobby players
        /// </summary>
        IReadOnlyDictionary<string, IPlayer> Players { get; }

        /// <summary>
        /// Current drawing
        /// </summary>
        IReadOnlyList<ScribblersSharp.IDrawCommand> CurrentDrawing { get; }

        /// <summary>
        /// Game state
        /// </summary>
        EGameState GameState { get; }

        /// <summary>
        /// Gets invoked when "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        event NameChangeGameMessageReceivedDelegate OnNameChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when "update-players" game message has been received
        /// </summary>
        event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when "update-wordhint" game message has been received
        /// </summary>
        event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// Gets invoked when "message" game message has been received
        /// </summary>
        event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "non-guessing-player-message" game message has been received
        /// </summary>
        event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "system-message" game message has been received
        /// </summary>
        event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "line" game message has been received
        /// </summary>
        event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when "fill" game message has been received
        /// </summary>
        event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when "clear-drawing-board" game message has been received
        /// </summary>
        event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when "your-turn" game message has been received
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "close-guess" game message has been received
        /// </summary>
        event CloseGuessGameMessageReceivedDelegate OnCloseGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "kick-vote" game message has been received.
        /// </summary>
        event KickVoteGameMessageReceivedDelegate OnKickVoteGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawer-kicked" game message has been received.
        /// </summary>
        event DrawerKickedGameMessageReceivedDelegate OnDrawerKickedGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "owner-change" game message has been received.
        /// </summary>
        event OwnerChangeGameMessageReceivedDelegate OnOwnerChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when "drawing" game message has been received
        /// </summary>
        event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// Creates a new client
        /// </summary>
        void CreateClient();

        /// <summary>
        /// Destroys current client
        /// </summary>
        void DestroyClient();

        /// <summary>
        /// Leaves lobby
        /// </summary>
        void LeaveLobby();

        /// <summary>
        /// Enters a lobby asynchronously
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>"true" if successful, otherwise "false" as a task</returns>
        Task<bool> EnterLobbyAsync(string lobbyID, string username);

        /// <summary>
        /// Creates a lobby asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is public</param>
        /// <param name="maximalPlayers">Maximal amount of players allowed</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Maximal amout of rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom word chance</param>
        /// <param name="isVotekickEnabled">Is votekick enabled</param>
        /// <param name="clientsPerIPLimit">Allowed clients per IP limit</param>
        /// <returns>"true" if successful, otherwise "false" as a task</returns>
        Task<bool> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayers, ulong drawingTime, uint rounds, IReadOnlyList<string> customWords, uint customWordsChance, bool isVotekickEnabled, uint clientsPerIPLimit);

        /// <summary>
        /// Gets server statistics asynchronously
        /// </summary>
        /// <returns>Server statistics task</returns>
        Task<IServerStatistics> GetServerStatisticsAsync();

        /// <summary>
        /// Lists all public lobbies asynchronously
        /// </summary>
        /// <returns>Lobby views task</returns>
        Task<ILobbyViews> ListLobbiesAsync();

        /// <summary>
        /// Changes lobby rules asynchronously
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="roundCount">Round count</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Task</returns>
        Task ChangeLobbyRulesAsync(ELanguage? language = null, bool? isPublic = null, uint? maximalPlayerCount = null, ulong? drawingTime = null, uint? roundCount = null, IReadOnlyList<string> customWords = null, uint? customWordsChance = null, bool? isVotekickingEnabled = null, uint? clientsPerIPLimit = null);

        /// <summary>
        /// Sends a "start-game" message asynchronously
        /// </summary>
        void SendStartGameMessage();

        /// <summary>
        /// Sends a "line" game message asynchronously
        /// </summary>
        /// <param name="fromX">X component of start line position</param>
        /// <param name="fromY">Y component of start line position</param>
        /// <param name="toX">X component of end line position</param>
        /// <param name="toY">Y component of end line position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width in pixels</param>
        void SendLineGameMessage(float fromX, float fromY, float toX, float toY, ScribblersSharp.Color color, float lineWidth);

        /// <summary>
        /// Sends a "fill" game message asynchronously
        /// </summary>
        /// <param name="positionX">X component of fill start posiiton</param>
        /// <param name="positionY">Y component of fill start position</param>
        /// <param name="color"></param>
        void SendFillGameMessage(float positionX, float positionY, ScribblersSharp.Color color);

        /// <summary>
        /// Sends a "clear-drawing-board" game message asynchronously
        /// </summary>
        void SendClearDrawingBoardGameMessage();

        /// <summary>
        /// Sends a "message" game message asynchronously
        /// </summary>
        /// <param name="content">Message content</param>
        void SendMessageGameMessage(string content);

        /// <summary>
        /// Sends a "choose-word" game message asynchronously
        /// </summary>
        /// <param name="index">Word index</param>
        void SendChooseWordGameMessage(uint index);

        /// <summary>
        /// Sends a "name-change" game message asynchronously
        /// </summary>
        /// <param name="newUsername">New username</param>
        void SendNameChangeGameMessage(string newUsername);

        /// <summary>
        /// Sends a "request-drawing" game message asynchronously
        /// </summary>
        void SendRequestDrawingGameMessage();

        /// <summary>
        /// Sends a "kick-vote" game message asynchronously
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        void SendKickVoteGameMessage(IPlayer toKickPlayer);

        /// <summary>
        /// Sends a "keep-alive" game message asynchronously
        /// </summary>
        void SendKeepAliveGameMessage();
    }
}
